using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Control - By Input System
    /// </summary>
    public class Control_Input : IControl
    {
        public void OnEnter(Actor actor)
        {
            if (!actor.TryGetComponent<Controller_Input>(out var controller))
            {
                controller = actor.gameObject.AddComponent<Controller_Input>();
            }

            // 명시적으로 초기화 호출
            controller.SetActorBehaviours(actor);
        }

        public void OnExit(Actor actor)
        {
            if (actor.TryGetComponent<Controller_Input>(out var controller))
                Object.DestroyImmediate(controller);
        }
    }
}