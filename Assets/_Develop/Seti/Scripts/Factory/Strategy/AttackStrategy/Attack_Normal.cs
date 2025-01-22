using UnityEngine;

namespace Seti
{
    public class Attack_Normal : Attack_Base
    {
        // 추상화
        #region Abstract
        public override void Attack()
        {
            base.Attack();
            Attack_WithWeapon();
        }
        #endregion

        // 메서드
        #region Methods
        private void Attack_WithWeapon()
        {
            switch (condition.CurrentWeaponType)
            {
                case Condition_Actor.WeaponType.Sword:
                    Attack_Sword();
                    break;

                case Condition_Actor.WeaponType.Staff:
                    Attack_Staff();
                    break;

                case Condition_Actor.WeaponType.Fist:
                    Attack_Fist();
                    break;

                case Condition_Actor.WeaponType.Bow:
                    Attack_Bow();
                    break;

                default:
                    Attack_Null();
                    break;
            }
        }

        private void Attack_Sword()
        {
            Debug.Log("평타: Sword");
        }

        private void Attack_Staff()
        {
            Debug.Log("평타: Staff");
        }

        private void Attack_Fist()
        {
            Debug.Log("평타: Fist");
        }

        private void Attack_Bow()
        {
            Debug.Log("평타: Bow");
        }

        private void Attack_Null()
        {
            Debug.Log("평타: Null");
        }
        #endregion
    }
}