using UnityEngine;

namespace Seti
{
    public class Condition_IsPlayerEncounter : Node
    {
        private Transform enemy;
        private Transform player;
        private float detectionRange;

        public Condition_IsPlayerEncounter(Transform enemy, Transform player, float detectionRange)
        {
            this.enemy = enemy;
            this.player = player;
            this.detectionRange = detectionRange;
        }

        public override bool Execute()
        {
            return Vector3.Distance(enemy.position, player.position) < detectionRange;
        }
    }
}