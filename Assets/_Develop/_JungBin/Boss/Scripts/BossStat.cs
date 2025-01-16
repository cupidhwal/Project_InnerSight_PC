using UnityEngine;
using UnityEngine.Events;

public class BossStat : MonoBehaviour
{
    // Public Properties
    public static float Health { get; private set; } = 0;

    [SerializeField] private float maxHealth = 1000;
    [SerializeField] private float invulnerabilityTime = 2f; // 무적 시간

    private Animator animator;
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

    public void TakeDamage(float amount)
    {
        // 이미 죽었거나 무적 상태일 경우 데미지 처리 안 함
        if (Health <= 0 || isInvulnerable)
        {
            Debug.Log("데미지를 받을 수 없는 상태입니다.");
            return;
        }

        // 체력 감소
        Health -= amount;
        Debug.Log($"남아있는 체력: {Health}");

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
