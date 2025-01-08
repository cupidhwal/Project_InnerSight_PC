using UnityEngine;

namespace Seti
{
    public class State_Player : State_Common
    {
        public enum Weapon
        {
            Sword,
            Fist,
            Bow,
        }

        // 필드
        #region Variables
        private Weapon primaryWeapon;
        [SerializeField]
        private Weapon currentWeapon;
        public Weapon CurrentWeapon => currentWeapon;
        //public Weapon CurrentWeapon { get; private set; }
        #endregion

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
        private void Initialize()
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