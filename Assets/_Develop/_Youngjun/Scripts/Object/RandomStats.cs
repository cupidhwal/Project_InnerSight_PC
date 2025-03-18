using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public class RandomStats : MonoBehaviour
    {
        // 강화 수치 데이터 리스트
        public List<float> reinforceData = new List<float>();

        private string actionUI_Text = "";
        private bool isContact = false;

        InGameUI_RandomStats inGameUI_RandomStats;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            inGameUI_RandomStats = FindAnyObjectByType<InGameUI_RandomStats>();

            //ObjectFadeSystem.Instance.ObjectFadeIn_Particle(transform);

            for (int i = 0; i < StageManager.Instance.reinforceData.Count; i++)
            {
                reinforceData.Add(StageManager.Instance.reinforceData[i]); 
            }
        }

        private void Update()
        {
            if (isContact)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    ActionUIManager.Instance.DisableActionUI();

                    GetStatsReinforce();
                }
            }
        }
        void ActiveCollider()
        {
            transform.GetComponent<Collider>().enabled = true;
        }


        void GetStatsReinforce()
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isContact = true;

                actionUI_Text = "육신강화";

                ActionUIManager.Instance.EnableActionUI(actionUI_Text);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isContact = false;

                ActionUIManager.Instance.DisableActionUI();
            }
        }
    }
}