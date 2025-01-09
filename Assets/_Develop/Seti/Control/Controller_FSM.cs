using System;
using UnityEngine;

namespace Seti
{
    public class Controller_FSM : Controller_Base, IController
    {
        // �ʵ�
        #region Variables
        private StateMachine<Controller_FSM> stateMachine;  // FSM �ν��Ͻ�

        [SerializeField]
        private const float range_Detect = 15f;
        [SerializeField]
        private const float range_Attack = 1.5f;
        private float distance;
        #endregion

        // �Ӽ�
        #region Properties
        public StateMachine<Controller_FSM> StateMachine => stateMachine;
        public bool Detected => distance < range_Detect;
        public bool CanAttack => distance > range_Attack;
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
                new State_Idle()
            );

            // ���� �߰�
            stateMachine.AddState(new State_Chase());
            stateMachine.AddState(new State_Patrol());
            stateMachine.AddState(new State_Attack());

            // �ʱ�ȭ
            Player player = FindFirstObjectByType<Player>();
            distance = Vector3.Distance(player.transform.position, actor.transform.position);
        }

        private void Update()
        {
            // FSM ������Ʈ
            stateMachine.Update(Time.deltaTime);
        }
        #endregion
    }
}