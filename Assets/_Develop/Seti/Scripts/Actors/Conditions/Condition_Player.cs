namespace Seti
{
    public class Condition_Player : Condition_Actor
    {
        // 라이프 사이클
        #region Life Cycle
        protected override void Start()
        {
            base.Start();
        }
        #endregion

        // 메서드
        #region Methods
        public override void Initialize()
        {
            // 저장된 현재 장비
            primaryWeaponType = WeaponType.Sword;

            base.Initialize();

            // 초기 장비 설정
            ChangeWeapon(primaryWeaponType);
        }

        // 플레이어 제어권 여부
        public void PlayerSetActive(bool inAction) => this.inAction = inAction;

        // 초기 장비 설정
        public void ChangeWeapon(WeaponType weaponType)
        {
            currentWeaponType = weaponType;
            switch (weaponType)
            {
                case WeaponType.Sword:
                    currentWeapon = primaryWeapon;
                    break;
            }
        }
        #endregion
    }
}