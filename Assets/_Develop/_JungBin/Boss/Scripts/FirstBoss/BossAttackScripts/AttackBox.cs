using Seti;
using System.Collections;
using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// 공격시 켜지는 어택박스
    /// </summary>
    public class AttackBox : MonoBehaviour
    {
        [SerializeField] private Vector3 attackDirection;  // 공격 방향 (옵션)
        private BoxCollider boxCollider;
        [SerializeField] private float damageCooldown = 1f; // 데미지 입은 후 쿨타임
        private bool canTakeDamage = true; // 데미지 가능 여부

        private void OnTriggerEnter(Collider other)
        {
            // 플레이어의 Damagable 컴포넌트 확인
            Damagable playerDamagable = other.GetComponent<Damagable>();
            Actor actor = other.GetComponent<Actor>();
            if (playerDamagable != null && canTakeDamage)
            {
                // DamageMessage 생성
                Damagable.DamageMessage damageMessage = new Damagable.DamageMessage
                {
                    damager = this, // 공격자 (SlashAttack)
                    owner = actor, // 피해 대상 (플레이어)
                    amount = BossStageManager.Instance.Bosses[0].AttackDamage, // 데미지 양
                    direction = attackDirection.normalized, // 공격 방향 (옵션)
                    damageSource = transform.position, // 공격의 시작 위치
                    throwing = true, // 넉백 여부
                    stopCamera = false // 카메라 정지 여부
                };

                if (damageMessage.owner == null)
                {
                    return;
                }

                // 플레이어에게 데미지 적용
                playerDamagable.TakeDamage(damageMessage);
                StartCoroutine(ResetDamageCooldown());
            }
        }

        private IEnumerator ResetDamageCooldown()
        {
            yield return new WaitForSeconds(damageCooldown);
            canTakeDamage = true; // 쿨타임 후 다시 데미지 허용
        }
    }
}