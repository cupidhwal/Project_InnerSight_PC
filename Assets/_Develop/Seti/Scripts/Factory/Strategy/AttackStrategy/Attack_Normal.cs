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
            Condition_Actor condition = actor.ActorState as Condition_Actor;
            switch (condition.CurrentWeapon)
            {
                case Condition_Actor.Weapon.Sword:
                    Attack_Sword();
                    break;
                case Condition_Actor.Weapon.Fist:
                    Attack_Fist();
                    break;
                case Condition_Actor.Weapon.Bow:
                    Attack_Bow();
                    break;
            }
        }

        private void Attack_Sword()
        {
            Debug.Log("평타: Sword");
        }

        private void Attack_Fist()
        {
            Debug.Log("평타: Fist");
        }

        private void Attack_Bow()
        {
            Debug.Log("평타: Bow");
        }
        #endregion
    }
}