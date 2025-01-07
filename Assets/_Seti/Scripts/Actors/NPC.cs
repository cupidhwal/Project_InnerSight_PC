using UnityEngine;

namespace Seti
{
    public class NPC : Actor
    {
        // Ãß»óÈ­
        #region Abstract
        protected override State_Actor CreateState() => gameObject.AddComponent<State_NPC>();
        #endregion
    }
}