using UnityEngine;

namespace Seti
{
    public class NPC : Actor
    {
        // �߻�ȭ
        #region Abstract
        protected override State_Actor CreateState() => gameObject.AddComponent<State_NPC>();
        #endregion
    }
}