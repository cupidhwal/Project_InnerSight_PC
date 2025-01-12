using UnityEngine;

namespace Seti
{
    public class Condition_Player : Condition_Common
    {
        // 라이프 사이클
        #region Life Cycle
        private void Start()
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
            primaryWeapon = Weapon.Sword;

            // 초기 장비 설정
            currentWeapon = primaryWeapon;
        }

        public void ChangeWeapon(Weapon weapon) => currentWeapon = weapon;
        #endregion
    }
}