using UnityEngine;

namespace Seti
{
    public class Player : Actor
    {
        // �߻�ȭ
        #region Abstract
        protected override State_Actor CreateState() => gameObject.AddComponent<State_Player>();
        #endregion
    }
}