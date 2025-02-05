using UnityEngine;

namespace JungBin
{

    public class AxeSpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject axePrefab;
        [SerializeField] private Transform axeSpawnPoint;
        [SerializeField] private GameObject currentAxe;
        [SerializeField] private Transform targetPosition;

        public void SpawnAxe()
        {
            Vector3 spawnpoint = axeSpawnPoint.position;
            Quaternion rotation =  Quaternion.Euler(axeSpawnPoint.position.x, 0, 0);

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