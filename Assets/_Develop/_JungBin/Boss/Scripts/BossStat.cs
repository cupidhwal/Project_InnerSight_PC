using Seti;
using UnityEngine;
using UnityEngine.Events;

namespace JungBin
{
    public class BossStat : MonoBehaviour
    {
        // Public Properties
        [SerializeField] private string bossName; // 보스 이름

        [SerializeField] private float maxHealth = 500f; // 최대 체력
        [SerializeField] private float invulnerabilityTime = 3.5f; // 무적 시간
        [SerializeField] private float bossAttack = 25f; // 보스 공격력
        [SerializeField] private GameObject relicPrefab; // 드랍할 유물
        [SerializeField] private CapsuleCollider capsuleCollider;

        private Animator animator; // 보스 애니메이션
        private bool isBerserk = false; // 버서커 모드 여부
        private bool isInvulnerable = false; // 무적 여부
        private float timeSinceLastHit = 0f; // 무적 시간 관리

        private Damagable damagable; // Damagable 참조

        // Events
        public UnityAction OnDeath; // 보스가 죽었을 때
        public UnityAction OnBecomeVulnerable; // 무적 해제 시

        public float AttackDamage => bossAttack;
        public float MaxHealth => maxHealth; // 보스의 최대 체력
        public float Health => damagable != null ? damagable.CurrentHitPoints : 0; // 현재 체력
        public string BossName => bossName; // 보스 이름 접근자

        private void Start()
        {
            // 초기화
            animator = GetComponent<Animator>();
            damagable = GetComponent<Damagable>();

            if (damagable != null)
            {
                // Damagable 초기화
                damagable.OnDeath += HandleDeath;
                damagable.OnReceiveDamage += HandleReceiveDamage;
            }

            OnBecomeVulnerable += HandleBecomeVulnerable;

            ResetHealth();
            OnDeath += SpawnRelic;
        }

        private void Update()
        {
            // 무적 상태 타이머 처리
            if (isInvulnerable)
            {
                timeSinceLastHit += Time.deltaTime;
                if (timeSinceLastHit >= invulnerabilityTime)
                {
                    OnBecomeVulnerable?.Invoke(); // 무적 해제 이벤트 호출
                    isInvulnerable = false;
                    timeSinceLastHit = 0f;
                    
                }
            }

            // 버서커 모드 전환
            if (!isBerserk && Health <= maxHealth / 2)
            {
                EnterBerserkMode();
            }
        }

        // 버서커 모드 진입
        private void EnterBerserkMode()
        {
            isBerserk = true;
            capsuleCollider.enabled = false;
            isInvulnerable = true; // 버서커 모드 진입 중 무적
            animator.SetBool("IsBerserk", true);
            animator.SetTrigger("Berserk");
            animator.SetBool("IsWall", false);
            animator.SetBool("IsPlayer", false);
            Debug.Log("버서커 모드로 전환됨: 무적 상태 활성화");
        }

        // 보스 사망 처리
        private void HandleDeath()
        {
            if (Health > 0) return; // 이미 죽었으면 처리하지 않음

            animator.SetBool("IsDeath", true);
            animator.SetTrigger("Death");
            OnDeath?.Invoke(); // 죽음 이벤트 호출
            Debug.Log("보스가 사망했습니다.");
        }

        // 보스가 데미지를 받을 때
        private void HandleReceiveDamage()
        {
            Debug.Log("보스가 공격을 받았습니다!");
        }

        // 무적 상태 해제
        private void HandleBecomeVulnerable()
        {
            Debug.Log("보스의 무적 상태가 해제되었습니다.");
            capsuleCollider.enabled = true;
        }

        // 유물 드랍
        private void SpawnRelic()
        {
            capsuleCollider.enabled = false;
            Instantiate(relicPrefab, transform.position, Quaternion.identity, this.transform);
        }

        // 보스 체력 및 상태 초기화
        public void ResetHealth()
        {
            if (damagable != null)
            {
                damagable.ResetDamage();
            }

            isInvulnerable = false;
            isBerserk = false;
            timeSinceLastHit = 0f;
            animator.SetBool("IsBerserk", false);
            animator.SetBool("IsDeath", false);
        }

        /*// 치트로 즉시 죽이기
        public void CheatDie()
        {
            if (damagable != null)
            {
                damagable.TakeDamage(new Damagable.DamageMessage
                {
                    amount = damagable.CurrentHitPoints, // 현재 체력을 모두 감소시켜 즉시 죽임
                    damager = this,
                    owner = this,
                    damageSource = transform.position,
                    direction = Vector3.zero,
                    throwing = false,
                    stopCamera = false
                });
            }
        }*/
    }
}
