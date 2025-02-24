using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Interface
    /// </summary>
    public interface IBehaviour
    {
        void Initialize(Actor actor);
        Type GetBehaviourType();
    }

    [System.Serializable]
    public class Behaviour
    {
        [SerializeReference]
        public IBehaviour behaviour;

        // 생성자
        public Behaviour(IBehaviour behaviour)
        {
            this.behaviour = behaviour?? throw new System.ArgumentNullException(nameof(behaviour), "IBehaviour는 null일 수 없습니다.");
        }
    }

}