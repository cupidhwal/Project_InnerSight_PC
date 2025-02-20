using Seti;
using UnityEngine;

namespace JungBin
{

    public class ParticleDamage : MonoBehaviour
    {
        [SerializeField] private string targetTag = "Player"; // 타겟 태그 설정

        private void OnParticleCollision(GameObject other)
        {
            // 충돌한 대상이 플레이어인지 확인
            if (other.CompareTag(targetTag))
            {
                Debug.Log($"파티클이 {other.name}와 충돌!");

                // 플레이어의 Damagable 컴포넌트 가져오기
                Damagable playerDamagable = other.GetComponent<Damagable>();
                Actor actor = other.GetComponent<Actor>();
                if (playerDamagable != null)
                {
                    // 데미지 메시지 생성
                    Damagable.DamageMessage damageMessage = new Damagable.DamageMessage
                    {
                        damager = this, // 공격 주체 = 파티클 시스템 오브젝트
                        owner = actor, // 피해 대상 = 충돌한 플레이어
                        amount = BossStageManager.Instance.Bosses[0].AttackDamage, // 데미지 값
                        direction = (other.transform.position - transform.position).normalized, // 공격 방향
                        damageSource = transform.position, // 파티클 위치
                        throwing = false, // 넉백 여부
                        stopCamera = false // 카메라 정지 여부
                    };

                    // 데미지 적용
                    playerDamagable.TakeDamage(damageMessage);
                }
            }
        }

    }
}