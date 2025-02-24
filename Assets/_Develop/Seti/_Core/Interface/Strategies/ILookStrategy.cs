using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Look Behaviour의 Strategy Pattern
    /// </summary>
    public interface ILookStrategy : IStrategy
    {
        void Initialize(Actor actor, float mouseSensitivity = 0f);
        void Look(Vector2 readValue = default);
    }
}