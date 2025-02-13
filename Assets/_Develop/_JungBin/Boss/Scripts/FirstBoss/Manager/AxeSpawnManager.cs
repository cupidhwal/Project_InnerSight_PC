using UnityEngine;

namespace JungBin
{

    public class AxeSpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject axePrefab;
        [SerializeField] private GameObject berserkAxePrefab;
        [SerializeField] private Transform axeSpawnPoint;
        [SerializeField] private GameObject currentAxe;
        [SerializeField] private Transform targetPosition;
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void SpawnAxe()
        {
            Vector3 spawnpoint = axeSpawnPoint.position;
            Quaternion rotation = Quaternion.Euler(axeSpawnPoint.position.x, 0, 0);

            if (animator.GetBool("IsBerserk") == true)
            {
                Instantiate(berserkAxePrefab, spawnpoint, rotation, this.transform.parent);
                return;
            }

            Instantiate(axePrefab, spawnpoint, rotation, this.transform.parent);
            
        }

        public void AxeActiveTrue()
        {
            currentAxe.SetActive(true);
        }

        public void IsAttackFalse()
        {
            FirstBossManager.isAttack = false;
        }

        public void AxeActiveFalse()
        {
            currentAxe.SetActive(false);
            FirstBossManager.isAttack = true;
        }
    }
}