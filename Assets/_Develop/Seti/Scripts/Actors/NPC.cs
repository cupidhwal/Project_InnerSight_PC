using UnityEngine;

namespace Seti
{
    public class NPC : Actor
    {
        // 추상화
        #region Abstract
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_NPC>();
        #endregion
    }
}