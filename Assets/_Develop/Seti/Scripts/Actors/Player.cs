using UnityEngine;

namespace Seti
{
    public class Player : Actor
    {
        // �������̵�
        #region Override
        protected override State_Actor CreateState() => gameObject.AddComponent<State_Player>();
        #endregion
    }
}