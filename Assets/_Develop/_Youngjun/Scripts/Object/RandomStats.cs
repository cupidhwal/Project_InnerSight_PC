using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public class RandomStats : MonoBehaviour
    {
        // 강화 수치 데이터 리스트
        public List<float> reinforceData = new List<float>();

        InGameUI_RandomStats inGameUI_RandomStats;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            inGameUI_RandomStats = FindAnyObjectByType<InGameUI_RandomStats>();

            ObjectFadeSystem.Instance.ObjectFadeIn_Particle(transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatsManager.Instance.SetReinforceData();

                transform.GetComponent<Collider>().enabled = false;

                UIManager.Instance.statsReinforce.SetActive(true);
                inGameUI_RandomStats.RandomStatsReinforce();
                Time.timeScale = 0f;

                if (HiddenStageManager.Instance != null)
                {
                    HiddenStageManager.Instance.SelectReinforce();
                }
                else
                {
                    Destroy(gameObject);
                }

            }
        }
    }
}