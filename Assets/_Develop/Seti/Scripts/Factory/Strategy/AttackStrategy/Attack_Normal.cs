using UnityEngine;

namespace Seti
{
    public class Attack_Normal : Attack_Base
    {
        // �߻�ȭ
        #region Abstract
        public override void Attack()
        {
            base.Attack();
            if (actor is Player)
                Attack_Player();
            else Attack_Enemy();
        }
        #endregion

        // �޼���
        #region Methods
        private void Attack_Enemy()
        {
            Debug.Log("�Ϲ� ���� ��Ÿ");
        }

        private void Attack_Player()
        {
            State_Player state_Player = actor.ActorState as State_Player;
            switch (state_Player.CurrentWeapon)
            {
                case State_Player.Weapon.Sword:
                    Attack_Sword();
                    break;
                case State_Player.Weapon.Fist:
                    Attack_Fist();
                    break;
                case State_Player.Weapon.Bow:
                    Attack_Bow();
                    break;
            }
        }

        private void Attack_Sword()
        {
            Debug.Log("��Ÿ: Sword");
        }

        private void Attack_Fist()
        {
            Debug.Log("��Ÿ: Fist");
        }

        private void Attack_Bow()
        {
            Debug.Log("��Ÿ: Bow");
        }
        #endregion
    }
}