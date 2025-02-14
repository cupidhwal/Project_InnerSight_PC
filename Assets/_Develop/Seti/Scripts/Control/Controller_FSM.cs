using System;
using UnityEngine;

namespace Seti
{
    public class Controller_FSM : Controller_Base, IController
    {
        public enum EnemyState
        {
            Idle,
            Chase,
            Patrol,
            Stagger,
            BackOff,
            Encounter,
            Positioning,
            Attack_Magic,
            Attack_Normal,
            Dead
        }
        
        // 필드
        #region Variables
        private StateMachine<Controller_FSM> stateMachine;  // FSM 인스턴스
        [SerializeField]
        private EnemyState currentState;
        #endregion

        // 속성
        #region Properties
        public StateMachine<Controller_FSM> StateMachine => stateMachine;
        public EnemyState CurrentState => currentState;
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
            stateMachine.OnStateChanged += SwitchState;

            // 상태 추가
            AddStates();

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
        public void SwitchState(State<Controller_FSM> state)
        {
            // FSM 상태에 따라 동작 제어
            switch (state)
            {
                case Enemy_State_Idle:
                    currentState = EnemyState.Idle;
                    break;

                case Enemy_State_Chase:
                    currentState = EnemyState.Chase;
                    break;

                case Enemy_State_Patrol:
                    currentState = EnemyState.Patrol;
                    break;

                case Enemy_State_Stagger:
                    currentState = EnemyState.Stagger;
                    break;

                case Enemy_State_BackOff:
                    currentState = EnemyState.BackOff;
                    break;

                case Enemy_State_Encounter:
                    currentState = EnemyState.Encounter;
                    break;

                case Enemy_State_Positioning:
                    currentState = EnemyState.Positioning;
                    break;

                case Enemy_State_Attack_Magic:
                    currentState = EnemyState.Attack_Magic;
                    break;

                case Enemy_State_Attack_Normal:
                    currentState = EnemyState.Attack_Normal;
                    break;

                case Enemy_State_Dead:
                    currentState = EnemyState.Dead;
                    break;
            }
        }
        private void AddStates()
        {
            // 누구나 죽는다
            stateMachine.AddState(new Enemy_State_Dead());

            if (BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                if (moveBehaviour is Move move)
                {
                    if (move.HasStrategy<Move_Normal>() || move.HasStrategy<Move_Walk>())
                        stateMachine.AddState(new Enemy_State_Patrol());

                    if (move.HasStrategy<Move_Run>())
                        stateMachine.AddState(new Enemy_State_Encounter());

                    if (move.HasStrategy<Move_Nav>())
                    {
                        stateMachine.AddState(new Enemy_State_Chase());
                        stateMachine.AddState(new Enemy_State_BackOff());
                    }
                }
            }

            if (BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
            {
                if (attackBehaviour is Attack attack)
                {
                    if (attack.HasStrategy<Attack_Normal>() || attack.HasStrategy<Attack_Tackle>())
                        stateMachine.AddState(new Enemy_State_Attack_Normal());

                    if (attack.HasStrategy<Attack_Magic>())
                    {
                        stateMachine.AddState(new Enemy_State_Attack_Magic());
                        stateMachine.AddState(new Enemy_State_Positioning());
                    }
                }
            }

            if (BehaviourMap.TryGetValue(typeof(Stagger), out var staggerBehaviour))
            {
                if (staggerBehaviour is Stagger)
                    stateMachine.AddState(new Enemy_State_Stagger());
            }
        }

        private void BindFSMBehaviours()
        {
            // Move 행동 이벤트 바인딩
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                if (moveBehaviour is Move move)
                    stateMachine.OnStateChanged += move.SwitchStrategy;

            // Look 행동 이벤트 바인딩
            if (behaviourMap.TryGetValue(typeof(Look), out var lookBehaviour))
                if (lookBehaviour is Look look)
                    stateMachine.OnStateChanged += look.SwitchStrategy;

            // Attack 행동 이벤트 바인딩
            if (behaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                if (attackBehaviour is Attack attack)
                    stateMachine.OnStateChanged += attack.SwitchStrategy;

            // 다른 행동 이벤트 바인딩 가능
            // if (behaviourMap.TryGetValue(typeof(Jump), out var jumpBehaviour)) { ... }
        }
        #endregion
    }
}