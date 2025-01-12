using UnityEngine;

namespace Seti
{
    public class Enemy : Actor
    {
        public enum State
        {
            Idle,
            Chase,
            Patrol,
            Attack
        }

        // 필드
        #region Variables
        protected Player player;
        //public State state;

        [Header("Criteria : AI Behaviour")]
        [SerializeField]
        protected float range_Detect = 7.5f;
        [SerializeField]
        protected float range_Attack = 1f;
        [SerializeField]
        protected float distance;
        [SerializeField]
        protected State currentState;

        [Header("Criteria : AI Interval")]
        [SerializeField]
        protected float patrolInterval = 3f;
        [SerializeField]
        protected float attackInterval = 3f;
        #endregion

        // 속성
        #region Properties
        public Player Player => player;
        public bool Detected => distance <= range_Detect;
        public bool GoAttack => distance <= range_Attack;
        public bool CanAttack => distance <= range_Attack * 2f;
        public float PatrolInterval => patrolInterval;
        public float AttackInterval => attackInterval;
        #endregion

        // 오버라이드
        #region Override
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_Enemy>();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Start()
        {
            base.Start();

            // 초기화
            player = FindAnyObjectByType<Player>();
        }

        protected virtual void Update()
        {
            distance = Vector3.Distance(player.transform.position, transform.position);
        }
        #endregion

        // 메서드
        #region Methods
        public void SwitchState(State state) => currentState = state;
        #endregion
    }
}