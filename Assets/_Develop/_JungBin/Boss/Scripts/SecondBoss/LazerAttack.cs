using System.Collections;
using System.IO.Pipes;
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
        [SerializeField] private float turnSpeed = 30f;
        [SerializeField] Vector3 fireDirection;
        [SerializeField] GameObject Boss;

        private Transform player;
        private bool isFiring = false;
        private GameObject laserColliderObject;
        private CapsuleCollider laserCollider;

        public bool IsFiring => isFiring; // 외부에서 발사 상태 확인 가능

        private void Start()
        {
            player = BossStageManager.Instance.Player?.transform;

            if (player == null)
            {
                Debug.LogError("Player GameObject is null in BossStageManager!");
            }

            if (laserLine == null)
            {
                Debug.LogError("🚨 LineRenderer가 설정되지 않았습니다!");
            }

            fireDirection = firePoint.forward;

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


            Vector3 direction = player.position - transform.position;

            float elapsedTime = 0f;
            while (elapsedTime < laserDuration)
            {
                elapsedTime += Time.deltaTime;

                Vector3 fireDirection = firePoint.TransformDirection(Vector3.forward);

                Debug.Log($"🔥 firePoint.rotation: {firePoint.rotation.eulerAngles}");
                Debug.Log($"🔥 Boss.rotation: {Boss.transform.rotation.eulerAngles}");

                // 🔴 firePoint가 정확한 방향을 바라보는지 확인 (디버그용)
                Debug.DrawRay(firePoint.position, fireDirection * maxLaserDistance, Color.red, 2f);


                RotateTowardsPlayer(direction);

                // 🏹 Raycast로 충돌 확인 (벽이나 바닥 감지)
                RaycastHit hit;
                float laserLength = maxLaserDistance;

                if (Physics.Raycast(firePoint.position, fireDirection, out hit, maxLaserDistance))
                {
                    laserLength = hit.distance;
                }

                // 🔥 LineRenderer를 정확한 위치에 맞추기
                Vector3 startPos = firePoint.position;
                Vector3 endPos = startPos + (fireDirection * laserLength);

                laserLine.SetPosition(0, startPos);
                laserLine.SetPosition(1, endPos);

                /*// 🎯 레이저 방향 & 길이 조절
                AdjustLaser();
                AdjustAim();*/
                



                yield return null;
            }

            laserLine.enabled = false;
            isFiring = false;
        }

        private void AdjustLaser()
        {
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

        private void RotateTowardsPlayer(Vector3 direction) // 보스의 회전
        {
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
            Boss.transform.rotation = Quaternion.RotateTowards(Boss.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }



    }
}
