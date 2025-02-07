using Seti;
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

        private void OnTriggerEnter(Collider other)
        {
            // 플레이어의 Damagable 컴포넌트 확인
            Damagable playerDamagable = other.GetComponent<Damagable>();
            Actor actor = other.GetComponent<Actor>();
            if (playerDamagable != null)
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
                this.gameObject.SetActive(false);
            }
        }
    }
}