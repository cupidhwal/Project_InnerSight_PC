using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Seti
{
    public class HealthBar : MonoBehaviour
    {
        public Slider healthBar;
        public Player player;
        public TextMeshProUGUI healthText;

        private Damagable playerDamagable;

        private void Start()
        {
            playerDamagable = player.GetComponent<Damagable>();
        }

        private void Update()
        {
            UpdateHealth();
        }

        public void UpdateHealth()
        {
            healthBar.value = playerDamagable.CurrentHitRate;
            healthText.text = playerDamagable.CurrentHitPoints.ToString() + " / " + playerDamagable.MaxHitPoint.ToString();
        }
    }
}