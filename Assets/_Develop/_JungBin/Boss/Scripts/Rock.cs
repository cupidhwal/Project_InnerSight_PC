using JungBin;
using UnityEngine;

namespace JungBin
{

    public class Rock : MonoBehaviour
    {
        public float destroyDelay = 0f; // 충돌 후 제거까지 딜레이
        [SerializeField] private int attackDamage = 0;

        private void OnTriggerEnter(Collider other)
        {
            // 충돌 처리
            if (other.GetComponent<CapsuleCollider>() == GameManager.Instance.Player_HitCapsuleCollider)
            {
                GameManager.Instance.Player.TakeDamage(attackDamage);
            }

            // 파괴 효과 실행 후 제거
            Invoke(nameof(DestroyRock), destroyDelay);
        }

        private void DestroyRock()
        {
            // 파티클 효과 추가 가능
            Destroy(gameObject);
        }
    }
}