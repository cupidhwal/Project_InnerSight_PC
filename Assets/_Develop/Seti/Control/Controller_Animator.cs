using UnityEngine;

namespace Seti
{
    public class Controller_Animator : MonoBehaviour
    {
        // 필드
        #region Variables
        private StateMachine<Controller_Animator> aniMachine;
        private Transform actorTransform;
        #endregion

        // 속성
        #region Properties
        public Animator Animator { get; private set; }

        public bool IsMove { get; set; } = false;
        public bool IsDash { get; set; } = false;
        public bool IsDeath { get; set; } = false;
        public bool IsAttack { get; set; } = false;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected void Awake()
        {
            // 초기화
            if (!TryGetComponent<Actor>(out var actor))
                actor = GetComponentInParent<Actor>();
            actorTransform = actor.transform;

            // 애니메이션 컨트롤러 초기화
            Animator = GetComponent<Animator>();
            aniMachine = new StateMachine<Controller_Animator>(
                this,
                new AniState_Idle()
            );

            // 상태 추가
            aniMachine.AddState(new AniState_Move());
            aniMachine.AddState(new AniState_Dash());
            aniMachine.AddState(new AniState_Attack());
        }

        private void Update()
        {
            // FSM 업데이트
            aniMachine.Update(Time.deltaTime);
        }
        #endregion

        // 메서드
        #region Methods
        public void Initialize()
        {
            transform.position = actorTransform.position;
            transform.rotation = actorTransform.rotation;
        }
        #endregion
    }
}