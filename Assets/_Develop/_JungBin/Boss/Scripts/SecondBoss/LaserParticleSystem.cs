using JungBin;
using System.Collections;
using UnityEngine;

public class LaserParticleSystem : MonoBehaviour
{
    [SerializeField] private Transform firePoint;      // 레이저가 발사되는 위치
    [SerializeField] private ParticleSystem laserParticle; // 레이저 파티클 시스템
    [SerializeField] private LayerMask obstacleLayer; // 벽 감지 레이어
    [SerializeField] private float maxLaserDistance = 30f;  // 최대 레이저 사거리
    [SerializeField] private float laserDuration = 2f; // 레이저 유지 시간
    [SerializeField] private float turnSpeed = 30f;
    [SerializeField] GameObject Boss;

    private ParticleSystem.ShapeModule laserShape;  // 파티클의 Shape 모듈
    private Transform player;

    private void Start()
    {
        player = BossStageManager.Instance.Player?.transform;

        if (player == null)
        {
            Debug.LogError("Player GameObject is null in BossStageManager!");
        }

        if (laserParticle == null)
        {
            Debug.LogError("🚨 Laser Particle System이 설정되지 않았습니다!");
            return;
        }

        laserShape = laserParticle.shape; // Shape 모듈 가져오기
        laserParticle.Stop();
    }

    public void FireLaser()
    {
        StartCoroutine(FireLaserRoutine());
    }
    
    private IEnumerator FireLaserRoutine()
    {
        laserParticle.Play(); // 레이저 시작
        float elapsedTime = 0f;

        while (elapsedTime < laserDuration)
        {
            elapsedTime += Time.deltaTime;

            Vector3 direction = player.position - transform.position;

            RotateTowardsPlayer(direction);

            // 🔥 Raycast를 이용하여 충돌 지점 감지
            float laserLength = maxLaserDistance;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, maxLaserDistance, obstacleLayer))
            {
                laserLength = hit.distance; // 벽까지 거리로 레이저 길이 조정
            }


            // 📏 파티클 시스템 길이 조정 (Shape 크기 변경)
            laserShape.scale = new Vector3(0.2f , 0.2f, laserLength);
            laserShape.position = new Vector3(0, 0, laserLength / 2);

            // 💡 레이저 위치 조정 (앞쪽으로 이동)
            laserParticle.transform.position = firePoint.position;
            /*laserParticle.transform.position = firePoint.position + firePoint.forward * (laserLength / 2f);*/
            yield return null;
        }

        laserParticle.Stop(); // 레이저 종료
    }

    private void RotateTowardsPlayer(Vector3 direction) // 보스의 회전
    {
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
        Boss.transform.rotation = Quaternion.RotateTowards(Boss.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
