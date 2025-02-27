using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Tree의 Action - 플레이어를 향해 이동
    /// </summary>
    public class Action_Chase : Node
    {
        private float speed;

        public Action_Chase(Actor actor, Actor target, float speed)
        {
            this.actor = actor;
            this.target = target;
            this.speed = speed;
        }

        public override bool Execute()
        {
            if (actor.Controller.BehaviourMap.TryGetValue(typeof(Look), out var lookBehaviour))
                if (lookBehaviour is Look look)
                    look.FSM_LookInput();

            actor.Condition.IsMove = true;

            actor.transform.position = Vector3.MoveTowards(actor.transform.position, target.transform.position, speed * Time.deltaTime);
            return true;
        }
    }
}