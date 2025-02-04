using Seti;
using UnityEngine;

namespace JungBin
{
    public class SpawnAxeController : MonoBehaviour
    {
        [SerializeField] private float axeSpeed = 10f; // 도끼 속도
        private Vector3 targetPosition; // 목표 위치 (플레이어 위치)
        private Vector3 dir; // 이동 방향

        private void Start()
        {
            if (BossStageManager.Instance.Player.transform == null)
                return;
            // 플레이어의 위치를 목표로 설정
            targetPosition = BossStageManager.Instance.Player.transform.position;


            // 방향 계산 (플레이어 방향)
            dir = (targetPosition - transform.position).normalized;

            // 도끼는 Y축으로 이동하지 않음
            dir.y = 0f;

            Destroy(gameObject,2f);
        }

        private void Update()
        {
            // 도끼 이동
            //transform.position += dir * axeSpeed * Time.deltaTime;
            transform.Translate(dir * axeSpeed * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            // 플레이어와 충돌했을 때
            Damagable playerDamagable = other.GetComponent<Damagable>();
            Actor actor = other.GetComponent<Actor>();
            if (playerDamagable != null)
            {
                // DamageMessage 생성
                Damagable.DamageMessage damageMessage = new Damagable.DamageMessage
                {
                    damager = this, // 공격자 (도끼)
                    owner = actor, // 피해 대상 (플레이어)
                    amount = BossStageManager.Instance.Bosses[0].AttackDamage / 2, // 데미지 양
                    damageSource = transform.position, // 도끼의 현재 위치
                    direction = dir, // 이동 방향
                    throwing = true, // 투척 공격 여부
                    stopCamera = false // 카메라 멈춤 여부 (필요시 true로 변경)
                };

                if (damageMessage.owner == null)
                {
                    return;
                }

                // 플레이어에게 데미지 적용
                playerDamagable.TakeDamage(damageMessage);

                // 도끼 삭제
                Destroy(gameObject);
            }
            // 벽과 충돌했을 때
            else if (other.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
