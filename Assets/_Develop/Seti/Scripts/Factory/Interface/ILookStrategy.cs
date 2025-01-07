using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Look Behaviour¿« Strategy Pattern
    /// </summary>
    public interface ILookStrategy : IStrategy
    {
        void Initialize(Actor actor, float mouseSensitivity);
        void Look(Vector2 readValue);
    }
}