using UnityEngine;

namespace JungBin
{

    public class Player : MonoBehaviour
    {
        public static int Lives { get; private set; } = 1; // �⺻ ���� ��
        public bool IsAlive => Lives > 0;           // ���� ����

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