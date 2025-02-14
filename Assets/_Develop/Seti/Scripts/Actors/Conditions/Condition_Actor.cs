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
        protected Actor actor;
        protected Rigidbody rb;

        // 무기
        protected WeaponType primaryWeaponType;
        [SerializeField]
        protected WeaponType currentWeaponType;

        protected Weapon primaryWeapon;
        [SerializeField]
        protected Weapon currentWeapon;

        [SerializeField]
        protected bool inAction = false;
        #endregion

        // 속성
        #region Properties
        public Weapon CurrentWeapon => currentWeapon;
        public bool InAction => inAction;
        public bool IsGrounded { get; protected set; } = true;
        public bool CanMove { get; set; } = true;
        public bool IsStagger { get; set; } = false;
        public bool IsAttack { get; set; } = false;
        public bool IsMagic { get; set; } = false;
        public bool IsMove { get; set; } = false;
        public bool IsDead { get; set; } = false;
        public bool IsChase { get; set; } = false;

        public Vector3 AttackPoint { get; set; }    // 마우스로 클릭한 Attack 지점
        public Vector3 HitDirection { get; set; }   // 피격 방향
        public WeaponType CurrentWeaponType => currentWeaponType;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody>();

            // Damagable 클래스가 존재하면 상태 전환 리스너 구독
            if (TryGetComponent<Damagable>(out var damagable))
            {
                damagable.OnDeath += Die;
                damagable.OnDeath += StopRigidBody;
                damagable.OnReceiveDamage += StaggerOn;
                damagable.OnReceiveDamage += StopRigidBody;
                damagable.OnReleaveDamage += StaggerOff;
            }

            IsGrounded = true;
            inAction = true;
        }

        protected virtual void Awake()
        {
            // 초기화
            Initialize();
        }
        #endregion

        // 추상화
        #region Abstract
        public virtual void Initialize()
        {
            actor = GetComponent<Actor>();

            primaryWeapon = GetComponentInChildren<Weapon>();
            primaryWeapon.SetOwner(gameObject);
        }
        #endregion

        // 메서드
        #region Methods
        // 경직
        private void StaggerOn()
        {
            inAction = false;
            IsStagger = true;
        }
        private void StaggerOff()
        {
            inAction = true;
            IsStagger = false;
        }

        // 죽음
        protected virtual void Die()
        {
            IsDead = true;
            inAction = false;
        }

        // 명시적 정지
        protected void StopRigidBody()
        {
            if (rb == null) return;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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