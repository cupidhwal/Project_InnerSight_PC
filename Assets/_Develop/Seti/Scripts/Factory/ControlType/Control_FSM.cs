using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Control - By Finite State Machine
    /// </summary>
    public class Control_FSM : IControl
    {
        public void OnEnter(Actor actor)
        {
            if (!actor.TryGetComponent<Controller_FSM>(out var controller))
            {
                controller = actor.gameObject.AddComponent<Controller_FSM>();
            }

            // 명시적으로 초기화 호출
            controller.SetActorBehaviours(actor);
        }

        public void OnExit(Actor actor)
        {
            if (actor.TryGetComponent<Controller_FSM>(out var controller))
                Object.DestroyImmediate(controller);
        }
    }
}