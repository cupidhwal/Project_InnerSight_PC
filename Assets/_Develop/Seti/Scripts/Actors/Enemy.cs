using UnityEngine;

namespace Seti
{
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : Actor
    {
        public enum State
        {
            Idle,
            Chase,
            Patrol,
            Attack,
            Stagger,
            BackHome,
            Dead
        }

        // 필드
        #region Variables
        [SerializeField]
        [HideInInspector]
        protected Player player;

        [Header("Criteria : AI Behaviour")]
        [SerializeField]
        protected State currentState;
        [SerializeField]
        protected float range_Detect = 7.5f;
        [SerializeField]
        protected float range_Attack = 1f;
        [SerializeField]
        protected float range_BackOff = 10f;
        [SerializeField]
        protected float distancePlayer;     // Player와의 거리
        [SerializeField]
        protected float distancePlace;      // 원래 자리와의 거리

        [Header("Criteria : AI Interval")]
        [SerializeField]
        protected float patrolInterval = 3f;
        [SerializeField]
        protected float attackInterval = 3f;
        #endregion

        // 속성
        #region Properties
        public Player Player => player;
        public Vector3 HomePosition { get; private set; }
        public Vector3 AttackDirection { get; private set; }
        public bool Detected => Player && (distancePlayer <= range_Detect);
        public bool GoAttack => Player && (distancePlayer <= range_Attack);
        public bool CanAttack => Player && (distancePlayer <= range_Attack * 2f);
        public bool GoBackHome => distancePlace >= range_BackOff;
        public bool TooFarFromHome => distancePlace >= range_BackOff * 2f;
        public bool ImHome => distancePlace <= 1f;
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
            HomePosition = transform.position;
        }

        protected virtual void Update()
        {
            distancePlace = Vector3.Distance(HomePosition, transform.position);
            
            if (!player) return;

            distancePlayer = Vector3.Distance(player.transform.position, transform.position);
        }

        protected virtual void Awake()
        {
            // 참조
            player = FindAnyObjectByType<Player>();
        }
        #endregion

        // 메서드
        #region Methods
        public void SwitchState(State state) => currentState = state;
        #endregion
    }
}