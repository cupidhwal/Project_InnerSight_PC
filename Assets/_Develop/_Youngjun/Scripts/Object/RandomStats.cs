using UnityEngine;

namespace Noah
{
    public class RandomStats : MonoBehaviour
    {
        InGameUI_RandomStats inGameUI_RandomStats;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            inGameUI_RandomStats = FindAnyObjectByType<InGameUI_RandomStats>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                transform.GetComponent<Collider>().enabled = false;

                UIManager.Instance.statsReinforce.SetActive(true);
                inGameUI_RandomStats.RandomStatsReinforce();
                Time.timeScale = 0f;

                Destroy(gameObject);
            }
        }
    }
}