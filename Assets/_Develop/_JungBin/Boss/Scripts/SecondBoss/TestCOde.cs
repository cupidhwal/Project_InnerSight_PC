using System.Collections;
using UnityEngine;

namespace JungBin
{

    public class TestCOde : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private GameObject projectilePrefab; // 투사체 프리팹
        [SerializeField] private Transform leftHandSpawnPoint; // 왼손 투사체 생성 위치
        [SerializeField] private Transform rightHandSpawnPoint; // 오른손 투사체 생성 위치
        [SerializeField] private float fireSpeed = 15f; // 투사체 속도
        [SerializeField] private int defaultProjectileCount = 3; // 기본 투사체 개수
        [SerializeField] private float defaultFireRate = 0.3f; // 기본 발사 간격 (초)

        // 🔥 왼손에서 투사체 발사 (애니메이션 이벤트에서 호출)
        public void FireLeftHandProjectile()
        {
            FireProjectile(leftHandSpawnPoint);
        }

        // 🔥 오른손에서 투사체 발사 (애니메이션 이벤트에서 호출)
        public void FireRightHandProjectile()
        {
            FireProjectile(rightHandSpawnPoint);
        }

        // ❗ 여러 개의 투사체를 왼손에서 발사 (애니메이션 이벤트에서 호출)
        public void FireMultipleLeftHandProjectiles(int projectileCount, float fireRate)
        {
            StartCoroutine(FireProjectiles(leftHandSpawnPoint, projectileCount, fireRate));
        }

        // ❗ 여러 개의 투사체를 오른손에서 발사 (애니메이션 이벤트에서 호출)
        public void FireMultipleRightHandProjectiles(int projectileCount, float fireRate)
        {
            StartCoroutine(FireProjectiles(rightHandSpawnPoint, projectileCount, fireRate));
        }

        private IEnumerator FireProjectiles(Transform spawnPoint, int projectileCount, float fireRate)
        {
            for (int i = 0; i < projectileCount; i++)
            {
                FireProjectile(spawnPoint);
                yield return new WaitForSeconds(fireRate); // 일정한 간격으로 발사
            }
        }

        private void FireProjectile(Transform spawnPoint)
        {
            if (projectilePrefab == null || spawnPoint == null)
            {
                Debug.LogError("Projectile Prefab 또는 Spawn Point가 설정되지 않음!");
                return;
            }

            // 투사체 생성
            GameObject projectileInstance = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

            // 방향 설정 및 이동 시작
            Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(spawnPoint.forward * fireSpeed);
            }
        }
    }
}