using Seti;
using UnityEngine;
using UnityEngine.Events;

namespace JungBin
{
    public class BossStat : MonoBehaviour
    {
        // Public Properties
        public float Health { get; private set; } // 현재 체력
        [SerializeField] private float maxHealth = 1000f; // 최대 체력
        [SerializeField] private float invulnerabilityTime = 2f; // 무적 시간
        [SerializeField] private GameObject relicPrefab; // 드랍할 유물

        private Animator animator; // 보스 애니메이션
        private bool isBerserk = false; // 버서커 모드 여부
        private bool isInvulnerable = false; // 무적 여부
        private float timeSinceLastHit = 0f; // 무적 시간 관리

        // Events
        public UnityAction OnDeath; // 보스가 죽었을 때
        public UnityAction OnBecomeVulnerable; // 무적 해제 시

        private void Start()
        {
            // 초기화
            Health = maxHealth;
            animator = GetComponent<Animator>();

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
                    isInvulnerable = false;
                    timeSinceLastHit = 0f;
                    OnBecomeVulnerable?.Invoke(); // 무적 해제 이벤트 호출
                }
            }

            // 버서커 모드 전환
            if (!isBerserk && Health <= maxHealth / 2)
            {
                EnterBerserkMode();
            }
        }

        // 데미지 처리
        public void TakeDamage(Damagable.DamageMessage damageMessage)
        {
            // 무적 상태 또는 이미 사망한 경우 데미지 처리 안 함
            if (Health <= 0 || isInvulnerable)
            {
                Debug.Log("보스가 무적 상태이거나 이미 사망했습니다.");
                return;
            }

            // 체력 감소
            Health -= damageMessage.amount;
            Debug.Log($"보스가 {damageMessage.owner.name}에게 {damageMessage.amount} 데미지를 받았습니다. 남은 체력: {Health}");

            // 죽음 처리
            if (Health <= 0)
            {
                Die();
            }
        }

        private void EnterBerserkMode()
        {
            isBerserk = true;
            isInvulnerable = true; // 버서커 모드 진입 중 무적
            animator.SetBool("IsBerserk", true);
            Debug.Log("버서커 모드로 전환됨: 무적 상태 활성화");
        }

        private void Die()
        {
            if (Health > 0)
                return; // 이미 죽었으면 처리하지 않음

            animator.SetBool("IsDeath", true);
            OnDeath?.Invoke(); // 죽음 이벤트 호출
            Debug.Log("보스가 사망했습니다.");
        }

        private void SpawnRelic()
        {
            Instantiate(relicPrefab, transform.position, Quaternion.identity);
        }

        public void ResetHealth()
        {
            Health = maxHealth;
            isInvulnerable = false;
            isBerserk = false;
            timeSinceLastHit = 0f;
            animator.SetBool("IsBerserk", false);
            animator.SetBool("IsDeath", false);
            Debug.Log("보스 체력 및 상태가 초기화되었습니다.");
        }
    }
}
