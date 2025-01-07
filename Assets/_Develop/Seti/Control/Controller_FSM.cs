using System;

namespace Seti
{
    public class Controller_FSM : Controller_Base, IController
    {
        // 필드
        #region Variables
        private StateMachine<Controller_FSM> stateMachine; // FSM 인스턴스
        #endregion

        // 인터페이스
        #region Interface
        public Type GetControlType() => typeof(Control_FSM);
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected void Awake()
        {
            // FSM 초기화
            stateMachine = new StateMachine<Controller_FSM>(
                this,
                new FSM_Idle()
            );

            // 상태 추가
            stateMachine.AddState(new FSM_Move());
        }

        private void Update()
        {
            // FSM 업데이트
            stateMachine.Update();
        }
        #endregion
    }
}