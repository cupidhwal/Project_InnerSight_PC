using Seti;
using UnityEngine;

namespace JungBin
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f; // 투사체 속도
        [SerializeField] private float lifeTime = 5f; // 존재하는 시간
        [SerializeField] private int bossNumber = 0;
        private Vector3 direction; // 이동 방향
        [SerializeField] private GameObject brokenProjectile;
        [SerializeField] private GameObject unBrokenProjectile;

        private bool isHit = false;

        public void Initialize(Vector3 spawnDirection)
        {
            direction = spawnDirection.normalized;
            Destroy(gameObject, lifeTime);
            isHit = false;
        }

        private void Update()
        {
            if (isHit) return;
            transform.position += direction * speed * Time.deltaTime;
        }

        public void OnChildTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("플레이어 명중!");

                // 플레이어에게 데미지 적용
                Damagable playerDamagable = other.GetComponent<Damagable>();
                if (playerDamagable != null)
                {
                    Damagable.DamageMessage damageMessage = new Damagable.DamageMessage
                    {
                        damager = this,
                        owner = null, // 보스가 필요하면 보스 객체 추가
                        amount = BossStageManager.Instance.Bosses[bossNumber].AttackDamage / 2, // 데미지량 설정
                        direction = direction,
                        damageSource = transform.position,
                        throwing = true,
                        stopCamera = false
                    };

                    playerDamagable.TakeDamage(damageMessage);
                }

                ShowBrokenProjectile();
            }

            if (other.CompareTag("Wall"))
            {
                ShowBrokenProjectile();
            }
        }

        public void ShowBrokenProjectile()
        {
            isHit = true;
            brokenProjectile.SetActive(true);
            unBrokenProjectile.SetActive(false);

            Destroy(gameObject, 2f);
        }

    }
}
