using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Attack Behaviour의 Strategy Pattern
    /// </summary>
    public interface IAttackStrategy : IStrategy
    {
        void Initialize(Actor actor, float power = 10f);
        void Attack();
    }
}