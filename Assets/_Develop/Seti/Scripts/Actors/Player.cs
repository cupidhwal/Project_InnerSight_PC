using UnityEngine;

namespace Seti
{
    public class Player : Actor
    {
        // 오버라이드
        #region Override
        protected override State_Actor CreateState() => gameObject.AddComponent<State_Player>();
        #endregion
    }
}