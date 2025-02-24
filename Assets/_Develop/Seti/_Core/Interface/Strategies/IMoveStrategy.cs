using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Move Behaviour의 Strategy Pattern
    /// </summary>
    public interface IMoveStrategy : IStrategy
    {
        void Initialize(Actor actor);
        void Move(Vector2 readValue);
        void GetOverCurb(Collision collision);
    }
}