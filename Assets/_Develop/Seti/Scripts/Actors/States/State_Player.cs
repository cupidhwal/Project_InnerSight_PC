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

        // �ʵ�
        #region Variables
        private Weapon primaryWeapon;
        [SerializeField]
        private Weapon currentWeapon;
        public Weapon CurrentWeapon => currentWeapon;
        //public Weapon CurrentWeapon { get; private set; }
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            // �ʱ�ȭ
            Initialize();
        }
        #endregion

        // �޼���
        #region Methods
        private void Initialize()
        {
            // ����� ���� ���
            primaryWeapon = Weapon.Sword;

            // �ʱ� ��� ����
            currentWeapon = primaryWeapon;
        }

        public void ChangeWeapon(Weapon weapon) => currentWeapon = weapon;
        #endregion
    }
}