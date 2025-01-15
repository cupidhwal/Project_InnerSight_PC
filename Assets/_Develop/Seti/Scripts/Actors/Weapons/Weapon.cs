using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Weapon 추상 클래스
    /// </summary>
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void AttackEnter();
        public abstract void AttackExit();
    }
}