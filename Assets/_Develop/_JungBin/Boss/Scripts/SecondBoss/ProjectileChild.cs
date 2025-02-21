using UnityEngine;

namespace JungBin
{

    public class ProjectileChild : MonoBehaviour
    {
        private Projectile parentProjectile; // 부모 참조

        private void Start()
        {
            parentProjectile = GetComponentInParent<Projectile>(); // 부모 찾기
        }

        private void OnTriggerEnter(Collider other)
        {
            if (parentProjectile != null) // 부모가 존재하는 경우만 실행
            {
                parentProjectile.OnChildTriggerEnter(other); // 부모에게 이벤트 전달
            }
        }
    }
}