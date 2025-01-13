using UnityEngine;

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
            Debug.Log($"{damageMessage.damageSource}로부터 {damageMessage.amount}의 데미지를 입었습니다.");
        }

        // 사망 처리, 애니메이션, 연출, ...
        void Die(Damagable.DamageMessage damageMessage)
        {
            // TODO
            Debug.Log($"{damageMessage.damageSource}의 공격으로 사망하였습니다.");
        }
        #endregion
    }
}