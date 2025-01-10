using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// ���ݽ� ������ ���ùڽ�
    /// </summary>
    public class AttackBox : MonoBehaviour
    {
        [SerializeField] private int attackDamage = 0;
        private void OnTriggerEnter(Collider other)
        {
                if (other.GetComponent<CapsuleCollider>() == GameManager.Instance.Player_HitCapsuleCollider)
                {
                    Debug.Log("����");
                    GameManager.Instance.Player.TakeDamage(attackDamage);
                }
        }
    }
}