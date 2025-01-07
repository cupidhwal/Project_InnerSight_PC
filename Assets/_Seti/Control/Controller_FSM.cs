using System;

namespace Seti
{
    public class Controller_FSM : Controller_Base, IController
    {
        // �ʵ�
        #region Variables
        private StateMachine<Controller_FSM> stateMachine; // FSM �ν��Ͻ�
        #endregion

        // �������̽�
        #region Interface
        public Type GetControlType() => typeof(Control_FSM);
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        protected void Awake()
        {
            // FSM �ʱ�ȭ
            stateMachine = new StateMachine<Controller_FSM>(
                this,
                new FSM_Idle()
            );

            // ���� �߰�
            stateMachine.AddState(new FSM_Move());
        }

        private void Update()
        {
            // FSM ������Ʈ
            stateMachine.Update();
        }
        #endregion
    }
}