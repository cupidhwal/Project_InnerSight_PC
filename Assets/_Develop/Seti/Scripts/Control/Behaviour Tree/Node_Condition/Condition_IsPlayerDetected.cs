using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Tree의 Condition - 플레이어와의 거리 확인
    /// </summary>
    public class Condition_IsPlayerDetected : Node
    {
        private float detectionRange;

        public Condition_IsPlayerDetected(Actor actor, Actor target, float detectionRange)
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