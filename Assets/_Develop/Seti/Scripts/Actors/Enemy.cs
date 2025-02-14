using System.Collections;
using UnityEngine;
using Noah;
using Yoon;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Seti
{
    [RequireComponent(typeof(Condition_Enemy))]
    [RequireComponent(typeof(Damagable))]
    [RequireComponent(typeof(DamageText))]
    [RequireComponent(typeof(WorldSpaceHealthBar))]
    [RequireComponent(typeof(DropItem))]

    public class Enemy : Actor
    {
        // 필드
        #region Variables
        // 기본
        [SerializeField]
        [HideInInspector]
        protected Player player;
        protected IEnumerator chaseCor;     // 피격 시 실행할 코루틴
        protected NavMeshAgent agent;

        protected Vector3 previousTargetPos;
        protected Vector3 currentTargetPos;
        public UnityAction OnTargetMove;

        [Header("Calculator : AI Behaviour")]
        [SerializeField]
        protected float distancePlayer;     // Player와의 거리
        [SerializeField]
        protected float distancePlace;      // 원래 자리와의 거리

        [Header("Criteria : AI Behaviour")]
        [SerializeField]
        protected float range_Detect = 7.5f;
        [SerializeField]
        protected float range_Attack = 1f;
        [SerializeField]
        protected float range_Magic = 0f;
        [SerializeField]
        protected float range_BackOff = 10f;
        [SerializeField]
        protected float searchDuration = 3;     // 탐지 시간

        [Header("Criteria : AI Interval")]
        [SerializeField]
        protected float patrolInterval = 3f;
        [SerializeField]
        protected float attackInterval = 3f;
        [SerializeField]
        protected float magicInterval = 5f;

        [Header("Function : AI Behaviour")]
        [SerializeField]
        protected GameObject magicObject;
        [SerializeField]
        protected float magicDamage = 0f;
        #endregion

        // 속성
        #region Properties
        // 로직 기준
        public Player Player => player;
        public NavMeshAgent Agent => agent;
        public Vector3 HomePosition { get; private set; }

        // 상태 조건
        public bool IsThere
        {
            get
            {
                return player;
            }
        }
        public bool LockOn => player && (distancePlayer <= range_Attack * 2f);
        public bool Detected => player && (distancePlayer <= range_Detect);
        public bool CanMagic => player && magicObject && (distancePlayer <= range_Magic);
        public bool CanAttack => player && (distancePlayer <= range_Attack);
        public bool GoBackHome => distancePlace >= range_BackOff;
        public bool TooFarFromHome => distancePlace >= range_BackOff * 2f;
        public bool ImHome => distancePlace <= 1f;
        public float PatrolInterval => patrolInterval;
        public float AttackInterval => attackInterval;
        public float MagicInterval => magicInterval;
        public float MagicDamage => magicDamage;
        public float MagicRange => range_Magic;
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

            distancePlayer = player.Condition.IsDead ? 100f : Vector3.Distance(player.transform.position, transform.position);

            // 표적 이동 여부 체크
            WatchTarget();
        }

        protected virtual void Awake()
        {
            // 참조
            player = FindAnyObjectByType<Player>();
            agent = GetComponent<NavMeshAgent>();

            previousTargetPos = player.transform.position;
            currentTargetPos = player.transform.position;
        }
        #endregion

        // 메서드
        #region Methods
        private void SearchAndChase() => CoroutineExecutor(SearchAndChaseCor());

        private void WatchTarget()
        {
            currentTargetPos = player.transform.position;
            if (Vector3.Distance(currentTargetPos, previousTargetPos) > 1f)
            {
                previousTargetPos = currentTargetPos;
                OnTargetMove?.Invoke();
            }
        }

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

        public void MagicAttack()
        {
            if (player && magicObject && ComponentUtility.TryGetComponentInChildren<Hand_Magic_Attack>(transform, out var hand))
            {
                // 공격 방향
                Vector3 dir = player.transform.position - transform.position;
                Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0f, -2f, 0f);

                // 마법 시전
                GameObject go = Instantiate(magicObject, hand.transform.position, rot);
                go.GetComponent<MagicAttack_Particle>().SetAttacker(this);
                Destroy(go, 1f);
            }
        }
        #endregion
    }
}