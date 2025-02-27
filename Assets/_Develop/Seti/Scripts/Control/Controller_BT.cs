using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Seti
{
    public class Controller_BT : Controller_Base
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
        private Actor actor;
        private Actor target;

        [Header("Root Node : AI Behaviour")]
        [SerializeField]
        private Node root;

        [Header("Calculator : AI Behaviour")]
        [SerializeField]
        protected float distancePlace;          // 원래 자리와의 거리

        [Header("Criteria : AI Behaviour")]
        [SerializeField]
        protected float range_Detect = 7.5f;
        [SerializeField]
        protected float range_Attack = 1f;
        [SerializeField]
        protected float range_Magic = 0f;
        [SerializeField]
        protected float range_BackOff = 10f;
        protected float range_Obstacle = 2f;    // Fixed
        [SerializeField]
        protected float searchDuration = 3;     // 탐지 시간

        [Header("Criteria : AI Interval")]
        [SerializeField]
        protected float patrolInterval = 3f;
        [SerializeField]
        protected float attackInterval = 3f;
        [SerializeField]
        protected float magicInterval = 5f;
        #endregion

        // 인터페이스
        #region Interface
        public override Type GetControlType() => typeof(Control_BT);
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Start()
        {
            base.Start();

            // 최상위 Selector 노드
            Node_Selector selector = new();

            // 플레이어 공격 Sequence
            Node_Sequence attackSequence = new();
            attackSequence.AddChild(new Condition_IsPlayerEncounter(actor, target, range_Attack));
            attackSequence.AddChild(new Action_Attack(actor));

            // 플레이어 추적 Sequence
            Node_Sequence chaseSequence = new();
            chaseSequence.AddChild(new Condition_IsPlayerDetected(actor, target, range_Detect));
            chaseSequence.AddChild(new Action_Chase(actor, target, actor.Rate_Movement * actor.Magnification_WalkToRun));

            // Selector에 추가 (우선순위: 공격 > 추적 > Idle)
            selector.AddChild(attackSequence);
            selector.AddChild(chaseSequence);
            selector.AddChild(new Action_Idle(actor));

            root = selector;
        }

        protected override void Awake()
        {
            base.Awake();

            // 참조
            actor = GetComponent<Actor>();
            target = InitializeManager.Instance.Player;

            // 행동 이벤트 바인딩
            //BindFSMBehaviours();
        }

        protected override void Update()
        {
            //base.Update();

            root.Execute();
        }
        #endregion

        // 메서드
        #region Methods


        //public void SwitchState(State<Controller_FSM> state)
        //{
        //    // FSM 상태에 따라 동작 제어
        //    switch (state)
        //    {
        //        case Enemy_State_Idle:
        //            currentState = EnemyState.Idle;
        //            break;

        //        case Enemy_State_Chase:
        //            currentState = EnemyState.Chase;
        //            break;

        //        case Enemy_State_Patrol:
        //            currentState = EnemyState.Patrol;
        //            break;

        //        case Enemy_State_Stagger:
        //            currentState = EnemyState.Stagger;
        //            break;

        //        case Enemy_State_BackOff:
        //            currentState = EnemyState.BackOff;
        //            break;

        //        case Enemy_State_Encounter:
        //            currentState = EnemyState.Encounter;
        //            break;

        //        case Enemy_State_Positioning:
        //            currentState = EnemyState.Positioning;
        //            break;

        //        case Enemy_State_Attack_Magic:
        //            currentState = EnemyState.Attack_Magic;
        //            break;

        //        case Enemy_State_Attack_Normal:
        //            currentState = EnemyState.Attack_Normal;
        //            break;

        //        case Enemy_State_Dead:
        //            currentState = EnemyState.Dead;
        //            break;
        //    }
        //}

        //private void AddStates()
        //{
        //    // 누구나 죽는다
        //    stateMachine.AddState(new Enemy_State_Dead());

        //    if (BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
        //    {
        //        if (moveBehaviour is Move move)
        //        {
        //            if (move.HasStrategy<Move_Normal>() || move.HasStrategy<Move_Walk>())
        //                stateMachine.AddState(new Enemy_State_Patrol());

        //            if (move.HasStrategy<Move_Run>())
        //                stateMachine.AddState(new Enemy_State_Encounter());

        //            if (move.HasStrategy<Move_Nav>())
        //            {
        //                stateMachine.AddState(new Enemy_State_Chase());
        //                stateMachine.AddState(new Enemy_State_BackOff());
        //            }
        //        }
        //    }

        //    if (BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
        //    {
        //        if (attackBehaviour is Attack attack)
        //        {
        //            if (attack.HasStrategy<Attack_Normal>() || attack.HasStrategy<Attack_Tackle>())
        //                stateMachine.AddState(new Enemy_State_Attack_Normal());

        //            if (attack.HasStrategy<Attack_Magic>())
        //            {
        //                stateMachine.AddState(new Enemy_State_Attack_Magic());
        //                stateMachine.AddState(new Enemy_State_Positioning());
        //            }
        //        }
        //    }

        //    if (BehaviourMap.TryGetValue(typeof(Stagger), out var staggerBehaviour))
        //    {
        //        if (staggerBehaviour is Stagger)
        //            stateMachine.AddState(new Enemy_State_Stagger());
        //    }
        //}

        //private void BindFSMBehaviours()
        //{
        //    // Move 행동 이벤트 바인딩
        //    if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
        //        if (moveBehaviour is Move move)
        //            stateMachine.OnStateChanged += move.SwitchStrategy;

        //    // Look 행동 이벤트 바인딩
        //    if (behaviourMap.TryGetValue(typeof(Look), out var lookBehaviour))
        //        if (lookBehaviour is Look look)
        //            stateMachine.OnStateChanged += look.SwitchStrategy;

        //    // Attack 행동 이벤트 바인딩
        //    if (behaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
        //        if (attackBehaviour is Attack attack)
        //            stateMachine.OnStateChanged += attack.SwitchStrategy;

        //    // 다른 행동 이벤트 바인딩 가능
        //    // if (behaviourMap.TryGetValue(typeof(Jump), out var jumpBehaviour)) { ... }
        //}
        #endregion

        // 이벤트 메서드
        #region Event Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                target = other.GetComponent<Player>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                target = null;
        }
        #endregion
    }
}