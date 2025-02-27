using UnityEngine;

namespace Seti
{
    public class Attack_Slash_Multiple : Attack_Base
    {
        // 추상화
        #region Abstract
        public override void Attack()
        {
            condition.CanMove = false;
            condition.IsSlash_Multiple = true;
        }
        #endregion
    }
}