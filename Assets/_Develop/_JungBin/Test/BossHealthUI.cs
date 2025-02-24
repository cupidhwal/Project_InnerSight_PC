using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JungBin
{

    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI bossNameText; // 보스 이름 UI
        [SerializeField] private Slider healthSlider; // 보스 체력 슬라이더
        private BossStat currentBossStat; // 현재 보스의 상태 스크립트

        public void SetBoss(BossStat bossStat)
        {
            if (bossStat != null && healthSlider != null)
            {
                currentBossStat = bossStat;

                // 보스 이름 및 체력 UI 설정
                bossNameText.text = bossStat.BossName; // 보스 이름 표시
                healthSlider.maxValue = bossStat.MaxHealth;
                healthSlider.value = bossStat.Health;

            }
            else
            {
                Debug.LogError("BossStat 또는 HealthSlider가 올바르게 설정되지 않았습니다.");
            }
        }

        private void Update()
        {
            if (currentBossStat != null && healthSlider != null)
            {
                // 현재 보스 체력에 따라 슬라이더 업데이트
                healthSlider.value = currentBossStat.Health;
            }
        }
    }
}