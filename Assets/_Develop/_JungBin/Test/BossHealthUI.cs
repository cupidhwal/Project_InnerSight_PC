using UnityEngine;
using UnityEngine.UI;

namespace JungBin
{ 

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // 보스 체력 슬라이더
    [SerializeField] private BossStat bossStat;  // 보스 상태 스크립트

    private void Start()
    {
        if (bossStat != null && healthSlider != null)
        {
            // 슬라이더의 최대값과 현재값 초기화
            healthSlider.maxValue = bossStat.MaxHealth;
            healthSlider.value = bossStat.Health;
        }
        else
        {
            Debug.LogWarning("BossStat 또는 HealthSlider가 연결되지 않았습니다.");
        }
    }

    private void Update()
    {
        if (bossStat != null && healthSlider != null)
        {
            // 보스 체력에 따라 슬라이더 값 업데이트
            healthSlider.value = bossStat.Health;
        }
    }
}

}