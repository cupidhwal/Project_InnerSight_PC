using UnityEngine;

namespace Seti
{
    public class Player_Alter : Actor
    {
        // Ãß»óÈ­
        #region Abstract
        protected override State_Actor CreateState() => gameObject.AddComponent<State_Player>();
        #endregion
    }
}