using UnityEngine;

namespace Seti
{
    public class Condition_IsPlayerEncounter : Node
    {
        private float detectionRange;

        public Condition_IsPlayerEncounter(Actor actor, Actor target, float detectionRange)
        {
            this.actor = actor;
            this.target = target;
            this.detectionRange = detectionRange;
        }

        public override bool Execute()
        {
            return Vector3.Distance(actor.transform.position, target.transform.position) < detectionRange;
        }
    }
}