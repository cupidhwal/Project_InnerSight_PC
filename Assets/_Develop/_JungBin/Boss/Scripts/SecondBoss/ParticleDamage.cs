using Seti;
using System.Collections;
using UnityEngine;

namespace JungBin
{

    public class ParticleDamage : MonoBehaviour
    {
        [SerializeField] private string targetTag = "Player"; // 타겟 태그 설정
        [SerializeField] private int bossNumber = 0;
        private bool canTakeDamage = true; // 데미지 가능 여부
        [SerializeField] private float damageCooldown = 0.5f; // 데미지 입은 후 쿨타임

        private void OnParticleCollision(GameObject other)
        {
            Debug.Log($"🔥 파티클 충돌 감지! 충돌한 대상: {other.name}");

            // 충돌한 대상이 플레이어인지 확인
            if (other.CompareTag(targetTag))
            {
                Debug.Log($"파티클이 {other.name}와 충돌!");

                // 플레이어의 Damagable 컴포넌트 가져오기
                Damagable playerDamagable = other.GetComponent<Damagable>();
                Actor actor = other.GetComponent<Actor>();
                if (playerDamagable != null && canTakeDamage)
                {
                    // 데미지 메시지 생성
                    Damagable.DamageMessage damageMessage = new Damagable.DamageMessage
                    {
                        damager = this, // 공격 주체 = 파티클 시스템 오브젝트
                        owner = actor, // 피해 대상 = 충돌한 플레이어
                        amount = BossStageManager.Instance.Bosses[bossNumber].AttackDamage, // 데미지 값
                        direction = (other.transform.position - transform.position).normalized, // 공격 방향
                        damageSource = transform.position, // 파티클 위치
                        throwing = false, // 넉백 여부
                        stopCamera = false // 카메라 정지 여부
                    };

                    // 데미지 적용
                    playerDamagable.TakeDamage(damageMessage);

                    canTakeDamage = false;

                    StartCoroutine(ResetDamageCooldown());
                }
            }
        }

        private IEnumerator ResetDamageCooldown()
        {
            yield return new WaitForSeconds(damageCooldown);
            canTakeDamage = true;

        }

    }
}