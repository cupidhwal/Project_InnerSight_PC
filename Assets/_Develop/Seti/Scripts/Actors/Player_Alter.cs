using UnityEngine;

namespace Seti
{
    public class Player_Alter : Actor
    {
        // �߻�ȭ
        #region Abstract
        protected override State_Actor CreateState() => gameObject.AddComponent<State_Player>();
        #endregion
    }
}