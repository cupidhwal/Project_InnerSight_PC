using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Seti
{
    /// <summary>
    /// HealthBar 기능 클래스
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        // 필드
        #region Variables
        // UI
        private Player player;
        private Slider healthSlider;
        private TextMeshProUGUI healthText;

        // 컴포넌트
        private Damagable damagable;

        // 값
        private readonly float fillSharpness = 10f;
        [SerializeField]
        private float fillDuration = 0.3f;
        private float currentHealth;

        // 기타
        private IEnumerator fillCor;
        #endregion


        private void Start()
        {
            player = FindAnyObjectByType<Player>();
            healthSlider = GetComponentInChildren<Slider>();
            healthText = GetComponentInChildren<TextMeshProUGUI>();

            damagable = player.GetComponent<Damagable>();
            damagable.OnReceiveDamage += UpdateHealth;
            damagable.OnResetDamage += UpdateHealth;
        }

        public void UpdateHealth()
        {
            float goalHealth = damagable.CurrentHitRate;
            healthText.text = damagable.CurrentHitPoints.ToString() + " / " + damagable.MaxHitPoint.ToString();

            // SmoothHealth
            UpdateSmoothHealth(goalHealth);
        }

        void UpdateSmoothHealth(float goalHealth)
        {
            if (fillCor != null)
            {
                StopCoroutine(fillCor);
                fillCor = null;
            }

            fillCor = SmoothHealth(goalHealth);
            StartCoroutine(fillCor);
        }

        IEnumerator SmoothHealth(float goalHealth)
        {
            // 연출
            float timeStamp = Time.time;
            while (timeStamp + fillDuration > Time.time)
            {
                currentHealth = Mathf.Lerp(currentHealth, goalHealth, fillSharpness * Time.deltaTime);
                healthSlider.value = currentHealth;
                yield return null;
            }

            // 마감
            currentHealth = goalHealth;
            healthSlider.value = goalHealth;

            // 후처리
            fillCor = null;
            yield break;
        }
    }
}