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
        [Header("Variables")]
        [SerializeField]
        private float maxHitPoints = 100;
        [SerializeField]
        private float currentHitPoints;
        [SerializeField]
        private float invulnerablityTime = 1f;
        [SerializeField]
        private float staggerDuration;

        [Range(0.0f, 360.0f)]
        public float hitAngle = 360f;
        [Range(0.0f, 360.0f)]
        public float hitForwardRotation = 360f;
        public List<MonoBehaviour> OnDamageMessageReceivers = new();

        protected float m_timeSinceLastHit = 0.0f;  // 무적 카운트다운
        protected float m_timeSinceStagger = 0.0f;  // 피격 카운트다운

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
        public float CurrentHitPoints => currentHitPoints;
        //public int CurrentHitPoints { get; private set; }       // 현재 체력
        //public Actor CurrentTarget { get; private set; }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 참조
            m_collider = GetComponent<Collider>();

            // 초기화
            ResetDamage();
        }

        private void Update()
        {
            // 피격 타이머
            if (actor && !actor.Condition.InAction)
            {
                m_timeSinceStagger += Time.deltaTime;
                if (m_timeSinceStagger > staggerDuration)
                {
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
        }

        private void LateUpdate()
        {
            if (schedule != null)
            {
                schedule();
                schedule = null;
            }
        }

        private void OnEnable()
        {
            // 씬의 모든 Actor(자신 제외)오브젝트 가져오기
            if (TryGetComponent<Actor>(out var actor))
            {
                /*damageControl = GetComponent<DamageControl>();
                OnDamageMessageReceivers.AddRange(damageControl.GetRelevantActors(damageControl));*/
                
                this.actor = actor;
                staggerDuration = actor.Stagger;

                if (actor is Player player)
                {
                    player.GetComponent<Enhance>().OnEnhance += ResetDamage;
                }

                if (actor is Enemy && OnDamageMessageReceivers[0] is Player targetPlayer)
                {
                    targetDamagable = targetPlayer.GetComponent<Damagable>();
                    OnReceiveDamage += targetPlayer.GetComponent<Condition_Player>().CurrentWeapon.AttackExit;
                }
            }
        }
        #endregion

        // 메서드
        #region Methods
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
            if (IsInvulnerable)
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
            currentHitPoints -= data.amount;

            if (currentHitPoints <= 0)
            {
                if (OnDeath != null)
                {
                    schedule += OnDeath.Invoke;
                }
            }
            else
            {
                OnReceiveDamage?.Invoke();
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
    }
}