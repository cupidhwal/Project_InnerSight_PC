using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Actor 추상 클래스
    /// </summary>
    public abstract class Condition_Actor : MonoBehaviour
    {
        public enum Weapon
        {
            Sword,
            Staff,
            Fist,
            Bow,
            NULL
        }

        // 필드
        #region Variables
        // 상태
        [SerializeField]
        private bool isGrounded;
        public bool IsGrounded => isGrounded;
        public bool IsAttack { get; set; }

        // 무기
        protected Weapon primaryWeapon;
        [SerializeField]
        protected Weapon currentWeapon;
        #endregion

        // 속성
        #region Properties
        public Weapon CurrentWeapon => currentWeapon;
        #endregion

        // 추상화
        #region Abstract
        protected abstract void Initialize();
        #endregion

        // 이벤트 메서드
        #region Event Methods
        // Collision 시리즈
        #region OnCollision
        private void OnCollisionChange(Collision collision, bool groundedState)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = groundedState;
            }
        }

        private void OnCollisionEnter(Collision collision) => OnCollisionChange(collision, true);
        private void OnCollisionStay(Collision collision) => OnCollisionChange(collision, true);
        //private void OnCollisionExit(Collision collision) => OnCollisionChange(collision, false);
        #endregion
        #endregion
    }
}