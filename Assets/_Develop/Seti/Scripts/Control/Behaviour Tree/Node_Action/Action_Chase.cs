using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Tree의 Action - 플레이어를 향해 이동
    /// </summary>
    public class Action_Chase : Node
    {
        private Transform enemy;
        private Transform player;
        private float speed;

        public Action_Chase(Transform enemy, Transform player, float speed)
        {
            this.enemy = enemy;
            this.player = player;
            this.speed = speed;
        }

        public override bool Execute()
        {
            enemy.position = Vector3.MoveTowards(enemy.position, player.position, speed * Time.deltaTime);
            return true;
        }
    }
}