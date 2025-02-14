using System.Collections;
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
        protected IEnumerator chaseCor;     // 피격 시 실행할 코루틴
        

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
        [SerializeField]
        protected float searchDuration = 3;     // 탐지 시간

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
        public bool CanAttack => Player && (distancePlayer <= range_Attack);
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

            // 이벤트 구독
            if (TryGetComponent<Damagable>(out var damagable))
                damagable.OnReceiveDamage += SearchAndChase;
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

        private void SearchAndChase() => CoroutineExecutor(SearchAndChaseCor());
        private IEnumerator SearchAndChaseCor()
        {
            // 탐지 거리를 기억하고 확 늘린다
            float initialRange = range_Detect;
            range_Detect = 100f;

            float elapsedTime = 0f;

            // 유지 시간
            float timeStamp = Time.time;
            while (timeStamp + searchDuration + 1f > Time.time)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / searchDuration;

                range_Detect = elapsedTime < searchDuration ? 100f : Mathf.Lerp(range_Detect, initialRange, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
            range_Detect = initialRange;

            yield break;
        }
        #endregion
    }
}