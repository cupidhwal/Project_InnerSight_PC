using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Move Behaviour¿« Strategy Pattern
    /// </summary>
    public interface IMoveStrategy : IStrategy
    {
        void Initialize(Actor actor, float speed);
        void Move(Vector2 readValue);
        void GetOverCurb(Collision collision);
    }
}