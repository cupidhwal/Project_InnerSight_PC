using UnityEngine;

namespace JungBin
{

    public class AxeSpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject axePrefab;
        [SerializeField] private Transform axeSpawnPoint;
        [SerializeField] private GameObject currentAxe;

        public void SpawnAxe()
        {
            Vector3 spawnpoint = axeSpawnPoint.position;
            Quaternion rotation = axeSpawnPoint.rotation;

            Instantiate(axePrefab, spawnpoint, rotation);
            
        }

        public void AxeActiveTrue()
        {
            currentAxe.SetActive(true);
        }

        public void AxeActiveFalse()
        {
            currentAxe.SetActive(false);
        }
    }
}