using UnityEngine;

namespace Seti
{
    public class Action_Attack : Node
    {
        public Action_Attack(Actor actor)
        {
            this.actor = actor;
        }

        public override bool Execute()
        {
            if (actor.Controller.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                if (attackBehaviour is Attack attack)
                    attack.OnAttack(true);
            return true;
        }
    }
}