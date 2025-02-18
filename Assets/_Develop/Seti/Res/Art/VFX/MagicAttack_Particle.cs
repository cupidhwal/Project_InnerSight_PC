using System.Collections;
using System.Xml;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    public class MagicAttack_Particle : MonoBehaviour
    {
        // 필드
        private Enemy attacker;
        private ParticleSystem magic;
        public Damagable.DamageMessage DamageData { get; private set; }

        // 라이프 사이클
        private void Start()
        {
            magic = GetComponent<ParticleSystem>();

            StartCoroutine(Timer_Col(0.6f));

            if (!magic.isPlaying)
                magic.Play();
        }

        public void SetAttacker(Enemy enemy)
        {
            attacker = enemy;
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

        IEnumerator Timer_Col(float t)
        {
            var col = magic.collision;
            col.collidesWith |= (1 << LayerMask.GetMask("Actor"));

            yield return new WaitForSeconds(t);
            col.collidesWith &= ~(1 << LayerMask.GetMask("Actor"));
        }
    }
}