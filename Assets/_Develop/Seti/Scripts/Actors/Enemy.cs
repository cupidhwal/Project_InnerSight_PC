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
            Attack,
            BackHome,
        }

        // 필드
        #region Variables
        protected Player player;
        //public State state;

        [Header("Variables")]
        [SerializeField]
        protected float speed_Walk = 2f;
        [SerializeField]
        protected float speed_Run = 3f;

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
        public bool ExcuteAttack { get; set; }
        public bool Detected => distancePlayer <= range_Detect;
        public bool GoAttack => distancePlayer <= range_Attack;
        public bool CanAttack => distancePlayer <= range_Attack * 3f;
        public bool GoBackHome => distancePlace >= range_BackOff;
        public bool TooFarFromHome => distancePlace >= range_BackOff * 2f;
        public bool ImHome => distancePlace <= 0.2f;
        public float Speed_Walk => speed_Walk;
        public float Speed_Run => speed_Run;
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

            // 참조
            player = FindAnyObjectByType<Player>();

            // 초기화
            HomePosition = transform.position;
        }

        protected virtual void Update()
        {
            distancePlayer = Vector3.Distance(player.transform.position, transform.position);
            distancePlace = Vector3.Distance(HomePosition, transform.position);
        }
        #endregion

        // 메서드
        #region Methods
        public void SwitchState(State state) => currentState = state;
        #endregion
    }
}