using Seti;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JungBin
{
    public class SpawnAxeController : MonoBehaviour
    {
        [SerializeField] private float axeSpeed = 10f; // 도끼 속도
        private Vector3 dir; // 이동 방향

        private void Start()
        {
            GameObject bossObject = GameObject.Find("FirstBoss");
            Transform targetPosition = null; // TargetPosition을 저장할 변수

            if (bossObject != null)
            {
                // "TargetPosition"이라는 이름을 가진 자식 오브젝트 찾기
                targetPosition = bossObject.transform.Find("TargetPosition");

                if (targetPosition != null)
                {
                    Debug.Log("TargetPosition 오브젝트를 찾음: " + targetPosition.name);
                }
                else
                {
                    Debug.LogWarning("TargetPosition 오브젝트를 찾을 수 없음!");
                }
            }
            else
            {
                Debug.LogError("BossStat을 가진 오브젝트를 찾을 수 없음!");
            }

            // 방향 계산 (플레이어 방향)
            dir = (targetPosition.position - transform.position).normalized;

            // 도끼는 Y축으로 이동하지 않음
            dir.y = 0f;


            Destroy(gameObject,2f);
        }

        private void Update()
        {
            // 도끼 이동
            //transform.position += dir * axeSpeed * Time.deltaTime;
            //transform.Translate(dir * axeSpeed * Time.deltaTime, Space.World);
            transform.Translate(dir * axeSpeed * Time.deltaTime);

            Vector3 fixedPosition = transform.position;
            fixedPosition.y = 1f; // Y값을 0으로 고정
            transform.position = fixedPosition;

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
