using System.Collections;
using UnityEngine;

namespace JungBin
{
    public class LazerAttack : MonoBehaviour
    {
        [SerializeField] private LineRenderer laserLine;  // 🔥 레이저 시각적 효과
        [SerializeField] private Transform firePoint;     // 🔥 레이저 시작 위치
        [SerializeField] private float maxLaserDistance = 50f;
        [SerializeField] private float laserDuration = 2f;
        [SerializeField] private float trackingSpeed = 2f; // 플레이어 피할 때 따라가는 속도

        private Transform player;
        private bool isFiring = false;
        private GameObject laserColliderObject;
        private CapsuleCollider laserCollider;

        public bool IsFiring => isFiring; // 외부에서 발사 상태 확인 가능

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾기
            if (laserLine == null)
            {
                Debug.LogError("🚨 LineRenderer가 설정되지 않았습니다!");
            }
        }

        public void FireLaser()
        {
            if (!isFiring)
            {
                StartCoroutine(FireLaserRoutine());
            }
        }

        private IEnumerator FireLaserRoutine()
        {
            isFiring = true;
            laserLine.enabled = true;

            float elapsedTime = 0f;
            while (elapsedTime < laserDuration)
            {
                elapsedTime += Time.deltaTime;

                // 🎯 레이저 방향 & 길이 조절
                AdjustLaser();
                AdjustAim();

                yield return null;
            }

            laserLine.enabled = false;
            isFiring = false;
        }

        private void AdjustLaser()
        {
            Vector3 fireDirection = firePoint.forward;
            RaycastHit hit;
            float laserLength = maxLaserDistance;

            if (Physics.Raycast(firePoint.position, fireDirection, out hit, maxLaserDistance))
            {
                laserLength = hit.distance;
            }

            // 🚀 레이저 길이 업데이트
            laserLine.SetPosition(0, firePoint.position);
            laserLine.SetPosition(1, firePoint.position + fireDirection * laserLength);

            // 🔥 충돌 감지용 Collider 업데이트
            SetupLaserCollider(firePoint.position, fireDirection, laserLength);
        }

        private void AdjustAim()
        {
            if (player == null) return;

            Vector3 targetDirection = (player.position - firePoint.position).normalized;
            firePoint.forward = Vector3.Lerp(firePoint.forward, targetDirection, trackingSpeed * Time.deltaTime);
        }

        private void SetupLaserCollider(Vector3 startPosition, Vector3 direction, float laserLength)
        {
            if (laserColliderObject == null)
            {
                laserColliderObject = new GameObject("LaserCollider");
                laserColliderObject.transform.SetParent(transform);
                laserCollider = laserColliderObject.AddComponent<CapsuleCollider>();
            }

            Vector3 midPoint = startPosition + direction * (laserLength / 2f);
            laserColliderObject.transform.position = midPoint;
            laserCollider.direction = 2; // Z축 기준
            laserCollider.height = laserLength;
            laserCollider.radius = 0.2f;
            laserCollider.isTrigger = true;
        }
        


    }
}
