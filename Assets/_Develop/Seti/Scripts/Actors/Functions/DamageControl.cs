using Noah;
using UnityEngine;
using Yoon;

namespace Seti
{
    public class DamageControl : MonoBehaviour, IMessageReceiver
    {
        // 필드
        #region Variables
        // 데미지 처리
        protected Damagable m_Damagable;
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
            // TODO
            //Debug.Log($"{damageMessage.owner.name}로부터 {damageMessage.amount}의 데미지를 입었습니다.");

            // 참조
            if (GetComponent<Actor>())
            {
                Condition_Actor condition = GetComponent<Condition_Actor>();
                condition.HitDirection = damageMessage.direction.normalized;
            }

            if (TryGetComponent<DamageText>(out var damageText))
            {
                damageText.OnTakeDamage(damageMessage);
            }
        }

        // 사망 처리, 애니메이션, 연출, ...
        void Die(Damagable.DamageMessage damageMessage)
        {
            // TODO
            //Debug.Log($"{damageMessage.owner.name}의 공격으로 사망하였습니다.");

            if (TryGetComponent<DamageText>(out var damageText))
            {
                damageText.OnTakeDamage(damageMessage);
            }

            Rigidbody rb = GetComponent<Rigidbody>();
            Collider collider = GetComponent<Collider>();

            if (rb != null)
            {
                rb.useGravity = false;
                collider.enabled = false;
            }

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
    }
}