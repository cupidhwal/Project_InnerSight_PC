using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    public class MagicAttack_Hitbox : MonoBehaviour
    {
        // 필드
        #region Variables
        // 공격자
        private Enemy attacker;
        private Vector3 hitDirection;
        [SerializeField]
        private float magicVelocity = 10f;
        #endregion

        // 속성
        public Damagable.DamageMessage DamageData { get; private set; }

        // 라이프 사이클
        private void Start()
        {
            attacker = GetComponentInParent<Actor>() as Enemy;
            hitDirection = attacker.Player.transform.position - attacker.transform.position;

            // 데미지 데이터 가공
            DamageData =  new()
            {
                damager = this,
                owner = attacker,
                amount = (int)attacker.MagicDamage,
                direction = hitDirection.normalized,
                damageSource = attacker.transform.position,
                throwing = true,
                stopCamera = false
            };

            transform.Translate(magicVelocity * hitDirection, Space.Self);
        }

        // 이벤트 메서드
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // 플레이어의 Damagable
                Damagable target = other.GetComponent<Damagable>();
                target.TakeDamage(DamageData);
            }
        }
    }
}