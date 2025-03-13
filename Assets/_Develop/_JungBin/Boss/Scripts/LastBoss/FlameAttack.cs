using Seti;
using System.Collections;
using UnityEngine;

namespace JungBin
{
    public class FlameAttack : MonoBehaviour
    {
        private CapsuleCollider flameCollider;
        [SerializeField] private float damageInterval = 1f; // 피해 간격 (0.5초마다 피해)
        [SerializeField] private LayerMask targetLayer; // 타겟이 될 레이어 (플레이어 감지)

        private float lastDamageTime = 0f; // 마지막으로 피해를 준 시간
        private bool isCanDamage;

        private void Start()
        {
            flameCollider = GetComponent<CapsuleCollider>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != targetLayer) return; // 대상이 타겟 레이어인지 확인

            if (isCanDamage) // 일정 간격으로 피해 적용
            {
                ApplyDamage(other);
                isCanDamage = false;

                StartCoroutine(DamageTimer());

            }
        }

        private void ApplyDamage(Collider target)
        {
            if (target.TryGetComponent<Damagable>(out var damagable))
            {
                Damagable.DamageMessage damageMessage = new Damagable.DamageMessage
                {
                    damager = this,
                    owner = target.GetComponent<Actor>(), // 피해 대상
                    amount = BossStageManager.Instance.Bosses[0].AttackDamage, // 데미지량
                    damageSource = transform.position,
                    direction = (target.transform.position - transform.position).normalized,
                    throwing = false,
                    stopCamera = false
                };

                Debug.Log($"🔥 {target.name}이(가) 화염 데미지를 입음!");
                damagable.TakeDamage(damageMessage);
            }
        }
        private IEnumerator DamageTimer()
        {
            yield return new WaitForSeconds( damageInterval );
            isCanDamage = true;
        }

        public void StartFlamethrower()
        {
            flameCollider.enabled = true;
        }

        public void StopFlamethrower()
        {
            flameCollider.enabled = false;
        }
    }

    
}



