using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Seti
{
    public partial class Damagable : MonoBehaviour
    {
        // 필드
        #region Variables
        public int maxHitPoints;
        public float invulnerablityTime;

        [Range(0.0f, 360.0f)]
        public float hitAngle = 360f;
        [Range(0.0f, 360.0f)]
        public float hitForwardRotation = 360f;

        public bool IsInvulnerable { get; set; }        // 무적 여부
        public int CurrentHitPoints { get; private set; }       // 현재 체력
        public List<MonoBehaviour> OnDamageMessageReceivers;

        protected float m_timeSinceLastHit = 0.0f;           // 무적 카운트다운

        public UnityAction OnDeath;
        public UnityAction OnReceiveDamage;
        public UnityAction OnHitWhileVulnerable;
        public UnityAction OnBecomeVulnerable;
        public UnityAction OnResetDamage;
        private System.Action schedule;

        protected Collider m_collider;
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
            CurrentHitPoints = maxHitPoints;
            IsInvulnerable = false;
            m_timeSinceLastHit = 0.0f;
            OnResetDamage?.Invoke();
        }

        // 데미지 처리
        public void TakeDamage(DamageMessage data)
        {
            // 이미 죽으면 더 이상 데미지를 입지 않는다
            if (CurrentHitPoints <= 0)
                return;

            // 무적 상태일 경우
            if (IsInvulnerable)
            {
                OnHitWhileVulnerable?.Invoke();
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
            CurrentHitPoints -= data.amount;

            if (CurrentHitPoints <= 0)
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
            var messageType = CurrentHitPoints <= 0 ?
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