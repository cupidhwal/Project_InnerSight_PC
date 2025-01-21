using UnityEngine;
using UnityEngine.UI;
using Yoon;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;          // 최대 체력
    private float currentHealth;

    public Slider healthBar;                // Health Bar Slider 연결
    public GameObject damageTextPrefab;     // DamageText 프리팹 연결
    public Transform fightWorldCanvas;      // DamageText가 생성될 부모 (FightWorldCanvas)

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        // 데미지 표시
        ShowDamageIndicator(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void ShowDamageIndicator(float damage)
    {
        if (damageTextPrefab != null && fightWorldCanvas != null)
        {
            // DamageText 프리팹 생성
            GameObject damageTextInstance = Instantiate(damageTextPrefab, fightWorldCanvas);

            // DamageText 위치를 적 머리 위로 설정
            damageTextInstance.transform.localPosition = new Vector3(0, 0.5f, 0); // 적 머리 위에 배치

            // DamageIndicator 스크립트에 데미지 값 전달
            DamageIndicator damageIndicator = damageTextInstance.GetComponent<DamageIndicator>();
            if (damageIndicator != null)
            {
                damageIndicator.SetDamage(damage);
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}
