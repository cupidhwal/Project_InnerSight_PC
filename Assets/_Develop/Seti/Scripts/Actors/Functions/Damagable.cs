using Noah;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Seti
{
    [RequireComponent(typeof(DamageControl))]
    public partial class Damagable : MonoBehaviour
    {
        // 필드
        #region Variables
        private Actor actor;
        public bool isDashInvulnerable = false;

        [Header("Variables")]
        [SerializeField]
        private float maxHitPoints = 100f;
        [SerializeField]
        private float currentHitPoints;
        [SerializeField]
        private float invulnerablityTime = 0.1f;
        [SerializeField]
        private float staggerDuration = 0.5f;

        [Range(0.0f, 360.0f)]
        public float hitAngle = 360f;
        [Range(0.0f, 360.0f)]
        public float hitForwardRotation = 360f;
        public List<MonoBehaviour> OnDamageMessageReceivers = new();

        protected float m_timeSinceLastHit = 0.0f;  // 무적 카운트다운
        protected float m_timeSinceStagger = 0.0f;  // 피격 카운트다운
        protected float m_timeSinceDash = 0.0f;     // 대시 카운트다운

        public UnityAction OnRevive;                // 이벤트: 부활
        public UnityAction OnDeath;                 // 이벤트: 죽음
        public UnityAction OnReceiveDamage;         // 이벤트: 피격
        public UnityAction OnReleaveDamage;         // 이벤트: 피격 상태 해제
        public UnityAction OnHitWhileInvulnerable;  // 이벤트: 무적 중 피격
        public UnityAction OnBecomeVulnerable;      // 이벤트: 무적 상태 해제
        public UnityAction OnResetDamage;           // 이벤트: 데미지 초기화
        private System.Action schedule;

        protected Collider m_collider;

        //
        private Damagable targetDamagable;
        private DamageControl damageControl;
        #endregion

        // 속성
        #region Properties
        public bool IsInvulnerable { get; set; }                // 무적 여부
        public float MaxHitPoint => maxHitPoints;               // 최대 체력
        public float CurrentHitPoints                           // 현재 체력
        {
            get
            {
                currentHitPoints = Mathf.Clamp(currentHitPoints, 0, float.MaxValue);
                return currentHitPoints;
            }
        }
        public float CurrentHitRate => currentHitPoints / maxHitPoints;     // 현재 체력 (%)
        /*public float InvulnerablityTime_Dash
        {
            get
            {
                if (actor is Player player)
                    return player.Dash_InvulnerablityTime;
                return invulnerablityTime;
            }
        }*/
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 참조
            m_collider = GetComponent<Collider>();

            // 초기화
            if (TryGetComponent<Actor>(out var actor))
            {
                this.actor = actor;
                staggerDuration = actor.Stagger;

                if (actor is Player player)
                {
                    player.GetComponent<Enhance>().OnEnhance += ResetDamage;
                    OnRevive += ResetDamage;
                }

                if (actor is Enemy enemy)
                {
                    Weapon enemyWeapon = enemy.GetComponentInChildren<Weapon>();
                    targetDamagable = enemy.Player.GetComponent<Damagable>();
                    //targetDamagable.OnReceiveDamage += enemyWeapon.AttackExit;
                }
            }

            // 초기화
            ResetDamage();
        }

        private void Update()
        {
            // 피격 타이머
            if (actor && !actor.Condition.InAction && !actor.Condition.IsDead)
            {
                m_timeSinceStagger += Time.deltaTime;
                if (m_timeSinceStagger > staggerDuration)
                {
                    if (actor.Condition.IsStagger)
                        OnReleaveDamage?.Invoke();

                    m_timeSinceStagger = 0f;
                }
            }

            // 무적 타이머
            if (IsInvulnerable)
            {
                m_timeSinceLastHit += Time.deltaTime;
                if (m_timeSinceLastHit > invulnerablityTime)
                {
                    IsInvulnerable = false;
                    OnBecomeVulnerable?.Invoke();

                    m_timeSinceLastHit = 0f;
                }
            }

            if (isDashInvulnerable)
            {
                Player player = actor as Player;
                m_timeSinceDash += Time.deltaTime;
                if (m_timeSinceDash > player.Dash_Duration)
                {
                    isDashInvulnerable = false;
                    m_timeSinceDash = 0f;
                }
            }
        }

        private void LateUpdate()
        {
            if (schedule != null)
            {
                schedule();
                schedule = null;
            }
        }
        #endregion

        // 메서드
        #region Methods
        private void Initialize()
        {
            if (actor)
            {
                maxHitPoints = actor.Health;
            }

            currentHitPoints = maxHitPoints;
            IsInvulnerable = false;
            m_timeSinceLastHit = 0.0f;
            OnResetDamage?.Invoke();
        }

        // 충돌체 활성/비활성
        public void SetColliderState(bool enabled)
        {
            m_collider.enabled = enabled;
        }

        // 데미지 데이터 초기화
        public void ResetDamage()
        {
            if (actor)
            {
                maxHitPoints = actor.Health;
            }

            currentHitPoints = maxHitPoints;
            IsInvulnerable = false;
            m_timeSinceLastHit = 0.0f;
            OnResetDamage?.Invoke();
        }

        // 데미지 처리
        public void TakeDamage(DamageMessage data)
        {
            // 이미 죽으면 더 이상 데미지를 입지 않는다
            if (currentHitPoints <= 0)
                return;

            // 무적 상태일 경우
            if (IsInvulnerable || isDashInvulnerable)
            {
                OnHitWhileInvulnerable?.Invoke();
                return;
            }

            // Hit 방향 구하기
            Vector3 forward = transform.forward;
            forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

            Vector3 positionToDamager = data.damageSource - transform.position;
            positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

            if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.5f)
                return;

            // 예외 처리가 모두 끝나면 데미지 처리
            IsInvulnerable = true;
            float damage;
            if (actor)
            {
                damage = (data.amount - actor.Defend);
                damage = Mathf.Clamp(damage, 1f, Mathf.Infinity);
            }
            else
            {
                damage = data.amount;
            }
            currentHitPoints -= damage;
            OnReceiveDamage?.Invoke();

            // 죽음 처리
            if (currentHitPoints <= 0)
            {
                if (OnDeath != null)
                {
                    schedule += OnDeath.Invoke;
                    StageManager.Instance.EnemyCount(gameObject);
                }
            }

            // 데미지 메시지 보내기
            var messageType = currentHitPoints <= 0 ?
                              GameMessageType.Dead :
                              GameMessageType.Damaged;
            for (int i = 0; i < OnDamageMessageReceivers.Count; i++)
            {
                var receiver = OnDamageMessageReceivers[i] as IMessageReceiver;
                receiver.OnReceiveMessage(messageType, this, data);
            }
        }
        #endregion

        // 유틸리티
        #region Utilities
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 forward = transform.forward;
            forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

            if (Event.current.type == EventType.Repaint)
            {
                Handles.color = Color.blue;
                Handles.ArrowHandleCap(0,
                                       transform.position,
                                       Quaternion.LookRotation(forward),
                                       1.0f,
                                       EventType.Repaint);
            }

            Handles.color = new Color(1.0f, 0.0f, 0.5f);
            forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;
            Handles.DrawSolidArc(transform.position,
                                 transform.up,
                                 forward,
                                 hitAngle,
                                 1.0f);
        }
#endif
        #endregion

        // 이벤트 메서드
        private void OnParticleCollision(GameObject other)
        {
            if (other.TryGetComponent<MagicAttack_Particle>(out var magic))
            {
                TakeDamage(magic.DamageData);
            }
        }
    }
}