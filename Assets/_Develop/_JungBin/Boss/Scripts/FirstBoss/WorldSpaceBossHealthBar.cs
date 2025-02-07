using Seti;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JungBin
{

    public class WorldSpaceBossHealthBar : MonoBehaviour
    {
        public Slider BossHealthBarSlider; // 헬스바 슬라이더

        private Damagable damagable; // Damagable 참조
        [SerializeField] private float targetHealth; // 목표 체력 값
        [SerializeField] private float lerpSpeed = 0.5f; // 체력 감소 속도

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            damagable = this.GetComponent<Damagable>();
            targetHealth = damagable.CurrentHitPoints;
        }

        // Update is called once per frame
        void Update()
        {
            if (damagable == null) return;

            // 체력 변경 감지 후 애니메이션 적용
            if (targetHealth != damagable.CurrentHitPoints)
            {
                targetHealth = damagable.CurrentHitPoints;
                StartCoroutine(LerpHealthBar());
            }
        }

        // 체력 값을 서서히 변경하는 코루틴
        private IEnumerator LerpHealthBar()
        {
            float startHealth = BossHealthBarSlider.value;
            float endHealth = damagable.CurrentHitRate; // 체력 비율

            float elapsedTime = 0f;
            while (elapsedTime < lerpSpeed)
            {
                elapsedTime += Time.deltaTime;
                BossHealthBarSlider.value = Mathf.Lerp(startHealth, endHealth, elapsedTime / lerpSpeed);
                yield return null;
            }

            // 최종값을 정확히 설정
            BossHealthBarSlider.value = endHealth;
        }
    }
}