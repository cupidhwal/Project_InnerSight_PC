using UnityEngine;

namespace Seti
{
    public class Attack_Weapon : Attack_Base
    {
        // 추상화
        #region Abstract
        public override void Attack()
        {
            Debug.Log("무기 기술");
        }
        #endregion
    }
}