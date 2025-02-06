using System.Collections;
using UnityEngine;
using Noah;
using Yoon;

namespace Seti
{
    public class DamageControl : MonoBehaviour, IMessageReceiver
    {
        // 필드
        #region Variables
        private Actor actor;

        // 데미지 처리
        protected Damagable m_Damagable;

        [Header("Criteria: Hit")]
        [SerializeField]
        private float KnockbackCoefficient = 4f;
        #endregion

        // 인터페이스
        #region Interface
        /*public bool IsRelevant(DamageControl damageControl)
        {
            Actor actor = GetComponent<Actor>();
            switch (actor)
            {
                case Player:
                    return actor is not NPC && actor != this;

                case Player_Alter:
                    return actor is Player;

                case NPC:
                    return actor is not Player && actor != this;

                case Enemy:
                    return actor is Player || actor is NPC;

                default:
                    return false;
            }
        }*/

        public void OnReceiveMessage(GameMessageType type, object sender, object msg)
        {
            Damagable.DamageMessage damageData = (Damagable.DamageMessage)msg;
            switch (type)
            {
                case GameMessageType.Damaged:
                    Damaged(damageData);
                    break;

                case GameMessageType.Dead:
                    Die(damageData);
                    break;
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void OnEnable()
        {
            actor = GetComponent<Actor>();

            m_Damagable = GetComponent<Damagable>();
            m_Damagable.OnDamageMessageReceivers.Add(this);
            m_Damagable.IsInvulnerable = true;
        }

        private void OnDisable()
        {
            m_Damagable.OnDamageMessageReceivers.Remove(this);
            m_Damagable = null;
        }
        #endregion

        // 메서드
        #region Methods
        // 데미지 처리, 애니메이션, 연출, ...
        void Damaged(Damagable.DamageMessage damageMessage)
        {
            // 참조
            if (actor)
            {
                // 넉백 기능
                Knockback(Knockback(damageMessage));
            }

            if (TryGetComponent<DamageText>(out var damageText))
            {
                damageText.OnTakeDamage(damageMessage);
            }
        }

        // 사망 처리, 애니메이션, 연출, ...
        void Die(Damagable.DamageMessage damageMessage)
        {
            // 최후의 데미지
            if (TryGetComponent<DamageText>(out var damageText))
            {
                damageText.OnTakeDamage(damageMessage);
            }

            // 콜라이더 제거
            Rigidbody rb = GetComponent<Rigidbody>();
            Collider collider = GetComponent<Collider>();
            if (rb != null)
            {
                rb.useGravity = false;
                collider.enabled = false;
            }

            // 이동 속도를 확실하게 제거
            if (actor)
            {
                Controller_Base controller = GetComponent<Controller_Base>();
                if (controller.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                {
                    if (moveBehaviour is Move move)
                    {
                        move.OnMove(Vector2.zero, false);
                    }
                }
            }

            // 플레이어 사망 시 재시작
            if (actor is Player)
                StageManager.Instance.ReStartGame();
        }

        // 씬 내의 대적자 액터 가져오기
        /*public List<DamageControl> GetRelevantActors(IMessageReceiver filter)
        {
            return FindObjectsByType<DamageControl>(FindObjectsSortMode.None)
                .Where(filter.IsRelevant)
                .ToList();
        }*/
        #endregion

        // 유틸리티
        // 넉백
        private void Knockback(IEnumerator knockbackCor)
        {
            StopAllCoroutines();
            StartCoroutine(knockbackCor);
        }
        IEnumerator Knockback(Damagable.DamageMessage damageMessage)
        {
            // 애니메이션의 Root Motion을 쓰지 않을 경우에만 실행
            if (actor.Controller_Animator.Animator.applyRootMotion) yield break;

            // 피해자
            Condition_Actor condition = GetComponent<Condition_Actor>();
            condition.HitDirection = damageMessage.direction.normalized;

            // 가해자
            Actor antagonist = damageMessage.owner.GetComponent<Actor>();

            // 초기 속도 설정 - 가해자 기준
            float elapsedTime = 0f;
            float atkDuration = 0.16f;
            float currentSpeed = KnockbackCoefficient *
                                 antagonist.Rate_Movement_Default;
            while (elapsedTime < atkDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / atkDuration;

                // Ease In-Out 적용
                currentSpeed = Mathf.Lerp(currentSpeed, 0, Mathf.SmoothStep(0f, 1f, t));
                actor.transform.Translate(currentSpeed * Time.deltaTime * antagonist.transform.forward, Space.World);

                yield return null;
            }

            yield break;
        }
    }
}