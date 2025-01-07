using UnityEngine;

namespace Seti
{
    public class Attack_Normal : Attack_Base
    {
        // 추상화
        #region Abstract
        public override void Attack()
        {
            Debug.Log("평타");
        }
        #endregion
    }
}