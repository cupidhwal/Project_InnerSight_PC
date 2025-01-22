using UnityEngine;
namespace Seti
{
    public enum AniState
    {
        Idle,
        Move,
        Dash,
        Attack,
        Stagger,
        Dead
    }

    public class Controller_Animator : MonoBehaviour
    {
        // 필드
        #region Variables
        // 컴포넌트
        private Controller_Base controller;
        private Damagable damagable;

        public AniState currentState;
        #endregion

        // 속성
        #region Properties
        public StateMachine<Controller_Animator> AniMachine { get; private set; }
        public Animator Animator { get; private set; }
        public Actor Actor { get; private set; }

        public float MoveSpeed { get; private set; }

        public bool IsMove { get; set; } = false;
        public bool IsDash { get; set; } = false;
        public bool IsDead { get; set; } = false;
        public bool IsChase { get; set; } = false;
        public bool IsMagic { get; set; } = false;
        public bool IsAttack { get; set; } = false;
        public bool IsStagger { get; set; } = false;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected void Start()
        {
            // 참조
            if (!TryGetComponent<Actor>(out var actor))
                actor = GetComponentInParent<Actor>();
            Actor = actor;

            if (!TryGetComponent<Controller_Base>(out var control))
                control = GetComponentInParent<Controller_Base>();
            controller = control;

            if (!TryGetComponent<Damagable>(out var damage))
                damage = GetComponentInParent<Damagable>();
            damagable = damage;

            // 이벤트 구독
            damagable.OnDeath += OnDie;

            // 애니메이션 컨트롤러 초기화
            Animator = GetComponent<Animator>();
            AniMachine = new StateMachine<Controller_Animator>(
                this,
                new AniState_Idle()
            );

            // 상태 추가
            AddStates();
        }

        private void Update()
        {
            MoveSpeed = CurrentSpeed();

            // FSM 업데이트
            AniMachine.Update(Time.deltaTime);
        }
        #endregion

        // 메서드
        #region Methods
        public void Initialize()
        {
            // 이건 모델 오브젝트를 루트 오브젝트의 자식으로 붙일 때만 사용한다
            /*transform.position = Actor.transform.position;
            transform.rotation = Actor.transform.rotation;*/
        }

        private void OnDie() => IsDead = true;

        private void AddStates()
        {
            // 누구나 죽는다
            AniMachine.AddState(new AniState_Die());

            if (controller.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                if (moveBehaviour is Move move)
                {
                    if (move.HasStrategy<Move_Normal>() || move.HasStrategy<Move_Walk>() || move.HasStrategy<Move_Run>())
                        AniMachine.AddState(new AniState_Move());

                    if (move.HasStrategy<Move_Dash>())
                        AniMachine.AddState(new AniState_Dash());
                }
            }

            if (controller.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
            {
                if (attackBehaviour is Attack attack)
                {
                    if (attack.HasStrategy<Attack_Normal>() || attack.HasStrategy<Attack_Tackle>())
                        AniMachine.AddState(new AniState_Attack_Melee());

                    if (attack.HasStrategy<Attack_Magic>())
                        AniMachine.AddState(new AniState_Attack_Magic());
                }
            }

            if (controller.BehaviourMap.TryGetValue(typeof(Stagger), out var staggerBehaviour))
            {
                if (staggerBehaviour is Stagger)
                    AniMachine.AddState(new AniState_Stagger());
            }
        }
        #endregion

        // 유틸리티
        #region Utilities
        public void MeleeAttackStart(int throwing = 0)
        {
            Actor.Condition.CurrentWeapon.BeginAttack(throwing != 0);
            Actor.Condition.IsAttack = true;
        }

        public void MeleeAttackEnd()
        {
            Actor.Condition.CurrentWeapon.EndAttack();
            Actor.Condition.IsAttack = false;
        }

        [SerializeField]
        private float forwardSpeed;
        private float CurrentSpeed()
        {
            if (IsMove)
                forwardSpeed = Mathf.Lerp(forwardSpeed, 4f, 10f * Time.deltaTime);
            else
                forwardSpeed = forwardSpeed > 0.01f ? Mathf.Lerp(forwardSpeed, 0f, 10f * Time.deltaTime) : 0f;

            int moveEff = IsChase ? 2 : 1;

            return moveEff * (Actor.Rate_Movement / Actor.Rate_Movement_Default) * forwardSpeed;
        }
        #endregion
    }
}