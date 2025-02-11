using UnityEngine;

namespace Seti
{
    public class Attack_Magic : Attack_Base
    {
        // 추상화
        #region Abstract
        public override void Attack()
        {
            condition.IsMagic = true;
        }

        public override void AttackExit()
        {
            condition.IsMagic = false;
        }
        #endregion
    }
}