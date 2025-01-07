using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Attack Behaviour¿« Strategy Pattern
    /// </summary>
    public interface IAttackStrategy : IStrategy
    {
        void Initialize(Actor actor, float power);
        void Attack();
    }
}