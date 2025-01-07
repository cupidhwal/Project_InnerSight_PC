using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Control - ´Ü¼ø
    /// </summary>
    public class Control_Stuff : IControl
    {
        public void OnEnter(Actor actor)
        {
            Debug.Log("Stuff Control Initialized");
        }

        public void OnExit(Actor actor)
        {
            Debug.Log("Stuff Control Exited");
        }
    }
}