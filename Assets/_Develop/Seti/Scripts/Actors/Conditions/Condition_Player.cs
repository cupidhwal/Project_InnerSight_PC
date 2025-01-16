namespace Seti
{
    public class Condition_Player : Condition_Actor
    {
        // 필드
        #region Variables
        private MeleeWeapon meleeWeapon;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Start()
        {
            // 초기화
            Initialize();
        }
        #endregion

        // 메서드
        #region Methods
        protected override void Initialize()
        {
            // 저장된 현재 장비
            primaryWeaponType = WeaponType.Sword;
            meleeWeapon = GetComponentInChildren<MeleeWeapon>();
            meleeWeapon.SetOwner(gameObject);

            // 초기 장비 설정
            ChangeWeapon(primaryWeaponType);

            // 게임 시작
            inAction = true;
        }

        public void PlayerSetActive(bool inAction) => this.inAction = inAction;

        public void ChangeWeapon(WeaponType weaponType)
        {
            currentWeaponType = weaponType;
            switch (weaponType)
            {
                case WeaponType.Sword:
                    currentWeapon = meleeWeapon;
                    break;
            }
        }
        #endregion
    }
}