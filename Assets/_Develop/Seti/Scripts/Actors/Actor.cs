using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Actor의 기본 정의
    /// </summary>
    public abstract class Actor : MonoBehaviour
    {
        // 필드
        #region Variables
        protected IControl control;
        [SerializeField]
        [HideInInspector]
        protected Blueprint_Actor blueprint;
        [SerializeField]
        [HideInInspector]
        protected State_Actor actorState;
        [SerializeField]
        [HideInInspector]
        protected List<Behaviour> behaviours = new();         // [직렬화 된 필드 - 읽기 전용 속성] 구조가 아니면 작동하지 않는다

        // 일반
        protected Controller_Animator animator;

        // 속성
        public Blueprint_Actor Origin => blueprint;
        public State_Actor ActorState => actorState;
        public List<Behaviour> Behaviours => behaviours;
        public Controller_Animator Controller_Animator => animator;
        #endregion

        // 추상화
        #region Abstract
        protected abstract State_Actor CreateState();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            animator = GetComponentInChildren<Controller_Animator>();
        }
        #endregion

        // 메서드
        #region Methods
        public void Initialize(Blueprint_Actor blueprint)
        {
            // Define Self
            this.blueprint = blueprint;

            // Check Actor State
            if (!actorState)
                actorState = CreateState();

            // Check Animator Controller
            Animator animator = GetComponentInChildren<Animator>();
            var controller = animator.transform.GetComponent<Controller_Animator>()
                ?? animator.transform.gameObject.AddComponent<Controller_Animator>();

            // Define Control
            SwitchController();
        }

        public void AddBehaviour(IBehaviour be)
        {
            Behaviour behaviour = new(be);
            behaviour.behaviour.Initialize(this);
            Behaviours.Add(behaviour);
        }

        private void SwitchController()
        {
            switch (blueprint.controlType)
            {
                case ControlType.Input:
                    SwitchControlType(new Control_Input());
                    break;

                case ControlType.FSM:
                    SwitchControlType(new Control_FSM());
                    break;

                case ControlType.Stuff:
                    SwitchControlType(new Control_Stuff());
                    break;
            }
        }

        private void SwitchControlType(IControl newControl)
        {
            // 현재 Actor에 부착된 컨트롤러 탐색
            if (TryGetComponent<IController>(out var _))
            {
                control?.OnExit(this);
            }
            control = newControl;
            control.OnEnter(this);
        }
        #endregion
    }
}