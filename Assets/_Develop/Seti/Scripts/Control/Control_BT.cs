using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Control - By Behaviour Tree
    /// </summary>
    public class Control_BT : IControl
    {
        public void OnEnter(Actor actor)
        {
            if (!actor.TryGetComponent<Controller_BT>(out var controller))
            {
                controller = actor.gameObject.AddComponent<Controller_BT>();
            }

            // 명시적으로 초기화 호출
            controller.SetBehaviours(actor);
        }

        public void OnExit(Actor actor)
        {
            if (actor.TryGetComponent<Controller_BT>(out var controller))
                Object.DestroyImmediate(controller);
        }
    }
}