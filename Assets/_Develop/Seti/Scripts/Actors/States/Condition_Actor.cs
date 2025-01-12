using UnityEngine;

namespace Seti
{
    public abstract class Condition_Actor : MonoBehaviour { }

    public abstract class Condition_Common : Condition_Actor
    {
        public enum Weapon
        {
            Sword,
            Fist,
            Bow,
            NULL
        }

        // 필드
        #region Variables
        // 상태
        public bool IsGrounded { get; set; }
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
        protected abstract void Initialize();

        // 이벤트 메서드
        #region Event Methods
        // Collision 시리즈
        #region OnCollision
        private void OnCollisionChange(Collision collision, bool groundedState)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                IsGrounded = groundedState;
            }
        }

        private void OnCollisionEnter(Collision collision) => OnCollisionChange(collision, true);
        private void OnCollisionStay(Collision collision) => OnCollisionChange(collision, true);
        private void OnCollisionExit(Collision collision) => OnCollisionChange(collision, false);
        #endregion
        #endregion
    }
}