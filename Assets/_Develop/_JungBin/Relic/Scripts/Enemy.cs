using UnityEngine;

namespace JungBin
{

    public class Enemy : MonoBehaviour
    {
        public GameObject relicPrefab; // ������ ����� ���� Prefab

        public void Die()
        {
            Instantiate(relicPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        //�ӽ� �ڵ�
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>())
            {
                Die();
            }
        }
    }
}