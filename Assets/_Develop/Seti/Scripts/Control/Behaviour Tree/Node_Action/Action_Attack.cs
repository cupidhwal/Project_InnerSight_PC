using UnityEngine;

namespace Seti
{
    public class Action_Attack : Node
    {
        private Enemy enemy;

        public Action_Attack(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override bool Execute()
        {
            if (enemy.Controller.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                if (attackBehaviour is Attack attack)
                    attack.OnAttack(true);
            return true;
        }
    }
}