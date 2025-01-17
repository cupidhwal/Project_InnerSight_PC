using UnityEngine;

namespace Seti
{
    public enum AniState
    {
        Idle,
        Move,
        Dash,
        Attack
    }

    public class Controller_Animator : MonoBehaviour
    {
        // 필드
        #region Variables
        private StateMachine<Controller_Animator> aniMachine;
        private Controller_Base controller;

        public AniState aniState;
        #endregion

        // 속성
        #region Properties
        public Actor Actor { get; private set; }
        public Animator Animator { get; private set; }

        public float MoveSpeed { get; private set; }

        public bool IsMove { get; set; } = false;
        public bool IsDash { get; set; } = false;
        public bool IsDeath { get; set; } = false;
        public bool IsAttack { get; set; } = false;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected void Start()
        {
            // 초기화
            if (!TryGetComponent<Actor>(out var actor))
                actor = GetComponentInParent<Actor>();
            Actor = actor;
            controller = GetComponentInParent<Controller_Base>();

            // 애니메이션 컨트롤러 초기화
            Animator = GetComponent<Animator>();
            aniMachine = new StateMachine<Controller_Animator>(
                this,
                new AniState_Idle()
            );

            // 임시로 플레이어만 확인
            if (Actor is Enemy) return;

            // 상태 추가
            AddStates();
        }

        private void Update()
        {
            // 임시로 플레이어만 확인
            if (Actor is Enemy) return;

            MoveSpeed = CurrentSpeed();

            // FSM 업데이트
            aniMachine.Update(Time.deltaTime);
        }
        #endregion

        // 메서드
        #region Methods
        public void Initialize()
        {
            transform.position = Actor.transform.position;
            transform.rotation = Actor.transform.rotation;
        }

        private void AddStates()
        {
            if (controller.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                if (moveBehaviour is Move move)
                {
                    if (move.HasStrategy<Move_Normal>())
                        aniMachine.AddState(new AniState_Move());

                    if (move.HasStrategy<Move_Dash>())
                        aniMachine.AddState(new AniState_Dash());
                }
            }

            if (controller.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
            {
                if (attackBehaviour is Attack attack)
                {
                    if (attack.HasStrategy<Attack_Normal>())
                        aniMachine.AddState(new AniState_Attack());

                    //if (attack.HasStrategy<Attack_Weapon>())

                    //if (attack.HasStrategy<Attack_Magic>())
                }
            }
        }
        #endregion

        // 유틸리티
        #region Utilities
        private float forwardSpeed;
        private float CurrentSpeed()
        {
            if (IsMove)
                forwardSpeed = Mathf.Lerp(forwardSpeed, 4f, 10f * Time.deltaTime);
            else
                forwardSpeed = forwardSpeed > 0.01f ? Mathf.Lerp(forwardSpeed, 0f, 10f * Time.deltaTime) : 0f;

            return (Actor.Rate_Movement / Actor.Rate_Movement_Default) * forwardSpeed;
        }
        #endregion
    }
}