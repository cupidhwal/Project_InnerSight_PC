using UnityEngine;

namespace Seti
{
    public class NPC : Actor
    {
        // 오버라이드
        #region Override
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_NPC>();
        public override bool IsRelevant(Actor actor) => actor is not Player && actor != this;
        #endregion
    }
}