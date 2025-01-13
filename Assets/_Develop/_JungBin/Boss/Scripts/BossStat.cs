using UnityEngine;

namespace JungBin
{

    public class BossStat : MonoBehaviour
    {
        public static float Health { get; private set; } = 0;   //기본 hp

        [SerializeField] private float maxHealth = 1000;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Health = maxHealth;
        }

        /*public void TakeDamage()*/

        
    }
}