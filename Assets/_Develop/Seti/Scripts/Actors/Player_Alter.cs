using UnityEngine;

namespace Seti
{
    public class Player_Alter : Enemy
    {
        // 추상화
        #region Abstract
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_Player>();
        public override bool IsRelevant(Actor actor) => actor is Player || actor is NPC;
        #endregion
    }
}