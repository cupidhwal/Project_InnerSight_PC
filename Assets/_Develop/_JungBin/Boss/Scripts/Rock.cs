using JungBin;
using UnityEngine;

namespace JungBin
{

    public class Rock : MonoBehaviour
    {
        public float destroyDelay = 0f; // �浹 �� ���ű��� ������
        [SerializeField] private int attackDamage = 0;

        private void OnTriggerEnter(Collider other)
        {
            // �浹 ó��
            if (other.GetComponent<CapsuleCollider>() == GameManager.Instance.Player_HitCapsuleCollider)
            {
                GameManager.Instance.Player.TakeDamage(attackDamage);
            }

            // �ı� ȿ�� ���� �� ����
            Invoke(nameof(DestroyRock), destroyDelay);
        }

        private void DestroyRock()
        {
            // ��ƼŬ ȿ�� �߰� ����
            Destroy(gameObject);
        }
    }
}