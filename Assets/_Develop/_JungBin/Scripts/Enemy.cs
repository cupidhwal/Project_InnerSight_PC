using UnityEngine;

namespace JungBin
{

    public class Enemy : MonoBehaviour
    {
        public GameObject relicPrefab; // 보스가 드롭할 유물 Prefab

        public void Die()
        {
            Instantiate(relicPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        //임시 코드
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>())
            {
                Die();
            }
        }
    }
}