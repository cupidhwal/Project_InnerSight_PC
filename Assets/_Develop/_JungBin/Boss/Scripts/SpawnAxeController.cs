using Seti;
using UnityEngine;

namespace JungBin
{
    public class SpawnAxeController : MonoBehaviour
    {
        [SerializeField] private float attackDamage = 10f; // 도끼의 데미지
        [SerializeField] private float axeSpeed = 10f; // 도끼 속도
        private Transform goalPosition; // 목표 위치 (플레이어 위치)
        private Vector3 dir; // 이동 방향

        private void Start()
        {
            // 플레이어의 위치를 목표로 설정
            goalPosition = GameManager.Instance.Player.transform;

            // 방향 계산 (플레이어 방향)
            dir = (goalPosition.position - transform.position).normalized;
        }

        private void Update()
        {
            // 도끼 이동
            transform.position += dir * axeSpeed * Time.deltaTime;

            // 도끼는 Y축으로 이동하지 않음
            dir.y = 0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            // 플레이어와 충돌했을 때
            Damagable playerDamagable = other.GetComponent<Damagable>();
            if (playerDamagable != null)
            {
                // DamageMessage 생성
                Damagable.DamageMessage damageMessage = new Damagable.DamageMessage
                {
                    damager = this, // 공격자 (도끼)
                    owner = GameManager.Instance.Actor, // 피해 대상 (플레이어)
                    amount = attackDamage, // 데미지 양
                    damageSource = transform.position, // 도끼의 현재 위치
                    direction = dir, // 이동 방향
                    throwing = true, // 투척 공격 여부
                    stopCamera = false // 카메라 멈춤 여부 (필요시 true로 변경)
                };

                // 플레이어에게 데미지 적용
                playerDamagable.TakeDamage(damageMessage);

                Debug.Log($"플레이어가 보스의 공격에 맞았습니다! 플레이어의 남은 체력 : {GameManager.Instance.Actor.Health}");

                // 도끼 삭제
                Destroy(gameObject);
            }
            // 벽과 충돌했을 때
            else if (other.CompareTag("Wall"))
            {
                Debug.Log("도끼가 벽에 부딪혔습니다!");
                Destroy(gameObject);
            }
        }
    }
}
