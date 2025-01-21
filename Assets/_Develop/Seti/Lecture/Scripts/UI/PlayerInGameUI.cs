using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayerInGameUI : MonoBehaviour
    {
        // 필드
        #region Variables
        public StatsObject statsObject;

        public Image healthBar;
        public Image manaBar;

        public TextMeshProUGUI levelText;
        public TextMeshProUGUI expText;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            healthBar.fillAmount = statsObject.HealthPercentage;
            manaBar.fillAmount = statsObject.ManaPercentage;

            levelText.text = statsObject.level.ToString();
            expText.text = statsObject.exp.ToString();
        }

        private void Update()
        {
            levelText.text = statsObject.level.ToString();
            expText.text = statsObject.exp.ToString();
        }

        private void OnEnable()
        {
            statsObject.OnChangedStats += OnChangedStats;
        }

        private void OnDisable()
        {
            statsObject.OnChangedStats -= OnChangedStats;
        }
        #endregion

        // 메서드
        #region Methods
        private void OnChangedStats(StatsObject stats)
        {
            healthBar.fillAmount = stats.HealthPercentage;
            manaBar.fillAmount = stats.ManaPercentage;
        }
        #endregion
    }
}