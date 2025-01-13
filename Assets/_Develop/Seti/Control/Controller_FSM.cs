using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    public class Controller_FSM : Controller_Base, IController
    {
        // 필드
        #region Variables
        private StateMachine<Controller_FSM> stateMachine;  // FSM 인스턴스
        #endregion

        // 속성
        #region Properties
        public StateMachine<Controller_FSM> StateMachine => stateMachine;
        public Dictionary<Type, IBehaviour> BehaviourMap => behaviourMap;
        #endregion

        // 인터페이스
        #region Interface
        public Type GetControlType() => typeof(Control_FSM);
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();

            // FSM 초기화
            stateMachine = new StateMachine<Controller_FSM>(
                this,
                new Enemy_State_Idle()
            );

            // 상태 추가
            stateMachine.AddState(new Enemy_State_Chase());
            stateMachine.AddState(new Enemy_State_Patrol());
            stateMachine.AddState(new Enemy_State_Attack());
            stateMachine.AddState(new Enemy_State_BackOff());

            // 행동 이벤트 바인딩
            BindFSMBehaviours();
        }

        protected override void Update()
        {
            base.Update();

            // FSM 업데이트
            stateMachine.Update(Time.deltaTime);
        }
        #endregion

        // 메서드
        #region Methods
        private void BindFSMBehaviours()
        {
            // Move 행동 이벤트 바인딩
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                if (moveBehaviour is Move move)
                    stateMachine.OnStateChanged += move.FSM_MoveSwitch;

            // Attack 행동 이벤트 바인딩
            if (behaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                if (attackBehaviour is Attack attack)
                    stateMachine.OnStateChanged += attack.FSM_AttackSwitch;

            // 다른 행동 이벤트 바인딩 가능
            // if (behaviourMap.TryGetValue(typeof(Jump), out var jumpBehaviour)) { ... }
        }
        #endregion
    }
}