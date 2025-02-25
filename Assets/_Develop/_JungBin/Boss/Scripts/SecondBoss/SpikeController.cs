using Seti;
using System.Collections;
using UnityEngine;

namespace JungBin
{
    public class SpikeController : MonoBehaviour
    {
        [Header("Attack Settings")]
        private Vector3 attackDirection = Vector3.up; // 공격 방향 (기본값)
        private float damageCooldown = 1f; // 데미지 쿨타임
        private bool canTakeDamage = true; // 데미지 가능 여부
        [SerializeField] private int bossNumber = 0;
        private Coroutine cooldownCoroutine;

        // **자식이 충돌을 감지하면 실행될 메서드**
        public void OnChildTriggerEnter(Collider other)
        {
            if (!canTakeDamage) return; // 이미 false면 실행 X

            Damagable playerDamagable = other.GetComponent<Damagable>();
            Actor actor = other.GetComponent<Actor>();

            if (playerDamagable != null)
            {
                float previousHP = playerDamagable.CurrentHitPoints; // 기존 체력 저장

                Damagable.DamageMessage damageMessage = new Damagable.DamageMessage
                {
                    damager = this,
                    owner = actor,
                    amount = BossStageManager.Instance.Bosses[bossNumber].AttackDamage,
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
