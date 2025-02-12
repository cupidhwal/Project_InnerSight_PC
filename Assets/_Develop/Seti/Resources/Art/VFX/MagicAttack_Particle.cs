using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    public class MagicAttack_Particle : MonoBehaviour
    {
        // 필드
        public Damagable.DamageMessage DamageData { get; private set; }

        // 라이프 사이클
        private void Start()
        {
            Enemy attacker = GetComponentInParent<Actor>() as Enemy;
            Vector3 hitDirection = attacker.Player.transform.position - attacker.transform.position;

            // 데미지 데이터 가공
            DamageData = new()
            {
                damager = this,
                owner = attacker,
                amount = (int)attacker.MagicDamage,
                direction = hitDirection.normalized,
                damageSource = attacker.transform.position,
                throwing = true,
                stopCamera = false
            };
        }
    }
}