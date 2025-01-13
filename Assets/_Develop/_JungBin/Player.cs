using Unity.Cinemachine;
using UnityEngine;

namespace JungBin
{

    public class Player : MonoBehaviour
    {
        public static int Lives { get; private set; } = 1; // �⺻ ���� ��

        public static int Health { get; private set; } = 100; // �⺻ ���� ��

        public void TakeDamage(int amount)
        {
            Health -= amount;
            Debug.Log($"������ ����. �����ִ� HP : {Health}");
            if( Health <= 0 )
            {
                Die();
            }
        }

        // ���� �߰� �޼���
        public void AddLife(int amount)
        {
            Lives += amount;
            Debug.Log($"���� �߰���: ���� ���� �� {Lives}");
        }

        // ��� �޼���
        public void Die()
        {
            if (Lives > 0) // ������ ���� ���� ���� ����
            {
                Lives--; // ���� ����
                //�̰��� ��� �ִϸ��̼� �߰�
                if (Lives == 0) // ���� �� ������ ������ ���� ó��
                {
                    //���� ���� UI ���
                    Debug.Log("�÷��̾� ����");
                }
                else
                {
                    //�һ� �ִϸ��̼� �߰�
                    Debug.Log($"�÷��̾� �һ� ���� ��� �� : {Lives}");
                }
            }
            else
            {
                Debug.Log("�÷��̾�� �̹� ���� �����Դϴ�.");
            }
        }
    }
}