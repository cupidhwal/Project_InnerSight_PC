using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// 공격시 켜지는 어택박스
    /// </summary>
    public class AttackBox : MonoBehaviour
    {
        [SerializeField] private int attackDamage = 0;
        private void OnTriggerEnter(Collider other)
        {
                if (other.GetComponent<CapsuleCollider>() == GameManager.Instance.Player_HitCapsuleCollider)
                {
                    Debug.Log("맞음");
                    GameManager.Instance.Player.TakeDamage(attackDamage);
                }
        }
    }
}