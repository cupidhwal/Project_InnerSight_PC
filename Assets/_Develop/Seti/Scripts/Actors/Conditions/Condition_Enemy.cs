namespace Seti
{
    public class Condition_Enemy : Condition_Actor
    {
        // 라이프 사이클
        #region Life Cycle
        protected override void Start()
        {
            base.Start();

            // 초기화
            Initialize();
        }
        #endregion

        // 메서드
        #region Methods
        public override void Initialize()
        {
            // 저장된 현재 장비
            primaryWeaponType = WeaponType.NULL;

            base.Initialize();

            // 초기 장비 설정
            currentWeaponType = primaryWeaponType;
            currentWeapon = primaryWeapon;
        }

        protected override void Die()
        {
            base.Die();
            Destroy(gameObject, 2);
        }
        #endregion
    }
}