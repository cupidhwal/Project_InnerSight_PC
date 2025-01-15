using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Actor 추상 클래스
    /// </summary>
    public abstract class Condition_Actor : MonoBehaviour
    {
        public enum WeaponType
        {
            Sword,
            Staff,
            Fist,
            Bow,
            NULL
        }

        // 필드
        #region Variables
        // 무기
        protected WeaponType primaryWeaponType;
        [SerializeField]
        protected WeaponType currentWeaponType;
        #endregion

        // 속성
        #region Properties
        public bool IsGrounded { get; protected set; }
        public bool IsAttack { get; set; }

        // Attack 지점
        public Vector3 AttactPoint { get; set; }
        public WeaponType CurrentWeaponType => currentWeaponType;
        #endregion

        protected virtual void Start()
        {
            GetComponent<Damagable>().OnDeath += Die;

            IsGrounded = true;
        }

        // 추상화
        #region Abstract
        protected abstract void Initialize();
        #endregion

        // 메서드
        #region Methods
        private void Die()
        {
            Destroy(this, 2);
        }
        #endregion

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
        //private void OnCollisionExit(Collision collision) => OnCollisionChange(collision, false);
        #endregion
        #endregion
    }
}