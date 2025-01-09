using System;
using UnityEngine;

namespace Seti
{
    public class Controller_FSM : Controller_Base, IController
    {
        // 필드
        #region Variables
        private StateMachine<Controller_FSM> stateMachine;  // FSM 인스턴스

        [SerializeField]
        private const float range_Detect = 15f;
        [SerializeField]
        private const float range_Attack = 1.5f;
        private float distance;
        #endregion

        // 속성
        #region Properties
        public StateMachine<Controller_FSM> StateMachine => stateMachine;
        public bool Detected => distance < range_Detect;
        public bool CanAttack => distance > range_Attack;
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
                new State_Idle()
            );

            // 상태 추가
            stateMachine.AddState(new State_Chase());
            stateMachine.AddState(new State_Patrol());
            stateMachine.AddState(new State_Attack());

            // 초기화
            Player player = FindFirstObjectByType<Player>();
            distance = Vector3.Distance(player.transform.position, actor.transform.position);
        }

        private void Update()
        {
            // FSM 업데이트
            stateMachine.Update(Time.deltaTime);
        }
        #endregion
    }
}