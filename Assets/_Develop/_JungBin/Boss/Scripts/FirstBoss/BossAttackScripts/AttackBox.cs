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
        private BoxCollider boxCollider;
        [SerializeField] private float damageCooldown = 1f; // 데미지 입은 후 쿨타임
        private bool canTakeDamage = true; // 데미지 가능 여부
        private Coroutine cooldownCoroutine;

        private void OnTriggerEnter(Collider other)
        {
            if (!canTakeDamage) return; // 이미 false면 추가 실행 안 함

            Damagable playerDamagable = other.GetComponent<Damagable>();
            Actor actor = other.GetComponent<Actor>();

            if (playerDamagable != null)
            {
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
        }
    }
}