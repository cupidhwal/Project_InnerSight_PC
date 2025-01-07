using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Interface
    /// </summary>
    public interface IBehaviour
    {
        void Upgrade(float increment);
        void Initialize(Actor actor);
        Type GetBehaviourType();
    }

    [System.Serializable]
    public class Behaviour
    {
        [SerializeReference]
        public IBehaviour behaviour;

        // ������
        public Behaviour(IBehaviour behaviour)
        {
            this.behaviour = behaviour?? throw new System.ArgumentNullException(nameof(behaviour), "IBehaviour�� null�� �� �����ϴ�.");
        }
    }

}