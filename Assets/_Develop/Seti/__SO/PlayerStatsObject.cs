using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "PlayerStatsObject", menuName = "Scriptable Objects/PlayerStatsObject")]
    public class PlayerStatsObject : ScriptableObject
    {
        public float hp = 100f;
        public float atk = 5f;
        public float def = 0f;
        public float moveSpeed = 5f;
        public float atkSpeed = 1f;
    }
}