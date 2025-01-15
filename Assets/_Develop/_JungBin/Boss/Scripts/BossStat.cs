using UnityEngine;

namespace JungBin
{

    public class BossStat : MonoBehaviour
    {
        public static float Health { get; private set; } = 0;   //기본 hp

        [SerializeField] private float maxHealth = 1000;
        private Animator animator;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Health = maxHealth;
        }

        private void Update()
        {
            if(Health <= maxHealth / 2)
            {
                animator.SetBool("IsBerserk", true);
            }
        }

        public void TakeDamage(float amount)
        {
            Debug.Log($"남아있는 체력 : {Health}");
            Health -= amount;

            if( Health <= 0 )
            {
                Die();
            }
        }

        private void Die()
        {
            animator.SetBool("IsDeath", true);
        }



        
    }
}