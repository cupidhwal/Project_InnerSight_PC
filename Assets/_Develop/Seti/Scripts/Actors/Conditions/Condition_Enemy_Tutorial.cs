using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Tutorial Enemy 전용
    /// </summary>
    public class Condition_Enemy_Tutorial : Condition_Enemy
    {
        protected override void Die()
        {
            IsDead = true;
            inAction = false;
        }
    }
}