using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Fly Function
    /// </summary>
    [System.Serializable]
    public class Fly : IBehaviour
    {
        // ���׷��̵�
        public void Upgrade(float increment)
        {

        }

        public void Initialize(Actor actor)
        {
            
        }

        public Type GetBehaviourType()
        {
            return typeof(Fly);
        }
    }
}