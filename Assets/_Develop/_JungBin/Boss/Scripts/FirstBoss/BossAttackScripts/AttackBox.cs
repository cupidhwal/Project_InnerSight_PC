using Seti;
using System.Collections;
using TMPro;
using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// 공격시 켜지는 어택박스
    /// </summary>
    public class AttackBox : MonoBehaviour
    {
        [SerializeField] private Vector3 attackDirection;  // 공격 방향 (옵션)
        [SerializeField] private float damageCooldown = 1f; // 데미지 입은 후 쿨타임
        private bool canTakeDamage = true; // 데미지 가능 여부
        private Coroutine cooldownCoroutine;
        [SerializeField] Animator animator;

        private void OnTriggerEnter(Collider other)
        {
            if (!canTakeDamage) return; // 이미 false면 추가 실행 안 함

            Damagable playerDamagable = other.GetComponent<Damagable>();
            Actor actor = other.GetComponent<Actor>();

            if (playerDamagable != null)
            {
                float previousHP = playerDamagable.CurrentHitPoints; // 데미지를 받기 전 체력 저장

                Damagable.DamageMessage damageMessage = new Damagable.DamageMessage
                {
                    damager = this,
                    owner = actor,
                    amount = BossStageManager.Instance.Bosses[0].AttackDamage,
                    direction = attackDirection.normalized,
                    damageSource = transform.position,
                    throwing = true,
                    stopCamera = false
                };

                if (damageMessage.owner == null) return;

                Debug.Log("플레이어에게 데미지 입힘");

                playerDamagable.TakeDamage(damageMessage);

                // 💡 데미지 적용 후 체력이 감소했는지 확인
                if (playerDamagable.CurrentHitPoints < previousHP)
                {
                    Debug.Log("플레이어가 실제로 데미지를 입음");
                    if (animator != null)
                    {
                        animator.SetBool("IsPlayer", true);
                    }
                }

                canTakeDamage = false;

                if (cooldownCoroutine != null)
                {
                    StopCoroutine(cooldownCoroutine);
                }
                cooldownCoroutine = StartCoroutine(ResetDamageCooldown());
            }
        }



        private IEnumerator ResetDamageCooldown()
        {
            yield return new WaitForSeconds(damageCooldown);
            canTakeDamage = true;
            if (animator != null)
            {
                animator.SetBool("IsPlayer", false);
            }
            
        }
    }
}