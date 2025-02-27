using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Tree의 Action - 기본 행동
    /// </summary>
    public class Action_Idle : Node
    {
        public Action_Idle(Actor actor)
        {
            this.actor = actor;
        }

        public override bool Execute()
        {
            actor.Condition.IsMove = false;
            return true;
        }
    }
}