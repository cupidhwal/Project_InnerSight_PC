using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Noah
{
    public class InGameUI_RandomStats : MonoBehaviour
    {
        private Dictionary<string, Sprite> statsDic = new Dictionary<string, Sprite>();
        private Dictionary<string, float> reinforceDic = new Dictionary<string, float>();

        // 강화 수치 데이터 리스트
        public List<float> reinforceData = new List<float>();
        // 스탯 이름
        public List<string> statsName = new List<string>();
        // stat 스프라이트 저장
        public List<Sprite> statsSprits = new List<Sprite>();
        // 랜덤으로 뽑힌 3가지 스탯 저장
        private List<string> randomStats = new List<string>();
        // 버튼 리스트
        public List<Button> btns = new List<Button>();

        // 강화 스텟 임시 보관
        private List<float> reinforceTmpData = new List<float>();

        private Transform contentsPar;

        [SerializeField] private Transform reinDataPar;

        public List<TMP_Text> statsData = new List<TMP_Text>();
        public List<TMP_Text> rein_Stats = new List<TMP_Text>();

        SaveLoadManager loadManager;
        PlayerStatsManager playerStatsManager;

        public void Init()
        {
            loadManager = SaveLoadManager.Instance;
            playerStatsManager = PlayerStatsManager.Instance;

            contentsPar = transform.GetChild(0).GetChild(0);

            for (int i = 0; i < playerStatsManager.dataList.Count; i++)
            {
                statsDic.Add(statsName[i], statsSprits[i]);
                reinforceDic.Add(statsName[i], reinforceData[i]);
                reinforceTmpData.Add(0f);

            }

            for (int i = 0; i < contentsPar.childCount; i++)
            {
                if (contentsPar.GetChild(i).GetChild(0).GetComponent<Button>() == null)
                {
                    continue;
                }
                else
                {
                    btns.Add(contentsPar.GetChild(i).GetChild(0).GetComponent<Button>());
                }

            }

            for (int i = 0; i < reinDataPar.childCount; i++)
            {
                statsData.Add(reinDataPar.GetChild(i).GetChild(0).GetComponent<TMP_Text>());
                rein_Stats.Add(reinDataPar.GetChild(i).GetChild(1).GetComponent<TMP_Text>());
            }

            SetUIData();

        }

        public void RandomStatsReinforce()
        {      
            while (randomStats.Count < 3)
            {
                int randomIndex = Random.Range(0, statsName.Count);

                if (!randomStats.Contains(statsName[randomIndex].ToString()))
                {
                    randomStats.Add(statsName[randomIndex].ToString());
                }
            }

            for (int i = 0; i < randomStats.Count; i++)
            {
                int index = i;

                string statKey = randomStats[index]; // 리스트에서 키 가져오기       

                // 랜덤 스킬 이미지 할당
                if (statsDic.TryGetValue(statKey, out Sprite statSprite))
                {
                    btns[i].transform.GetChild(0).GetComponent<Image>().sprite = statSprite;
                    //btns[i].transform.GetChild(1).GetComponent<TMP_Text>().text = 
                    //    statKey + $"{reinforceData[index]} 증가"; // 키를 텍스트로 표시

                    foreach (var kvp in reinforceDic)
                    {
                        if (statKey == kvp.Key)
                        {
                            btns[i].transform.GetChild(1).GetComponent<TMP_Text>().text =
                               $"{statKey} <color=#008BFF>{kvp.Value}</color> 증가";

                            btns[i].onClick.AddListener(() => AssignStatsToKey(kvp.Key, kvp.Value));
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"키 '{statKey}'가 statsDic에 존재하지 않습니다.");
                }

               
            }
        }

        void AssignStatsToKey(string _dicKey, float _reinData)
        {
            playerStatsManager.PlayerStatReinforce(_dicKey, _reinData);

            playerStatsManager.SetPlayerStat();

            UIBack();
        }

        public void UIBack()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            RemoveChangeListener();

            Time.timeScale = 1f;
        }

        void RemoveChangeListener()
        {
            for (int i = 0; i < btns.Count; i++)
            {
                btns[i].onClick.RemoveAllListeners();
            }

            randomStats.Clear();
        }

        public void SetUIData()
        {
            for (int i = 0; i < reinDataPar.childCount; i++)
            {
                Transform playerStats = reinDataPar.GetChild(i).GetChild(0);
                Transform reinStats = reinDataPar.GetChild(i).GetChild(1);

                playerStats.GetComponent<TMP_Text>().text = playerStatsManager.dataList[i].ToString();
                reinStats.GetComponent<TMP_Text>().text = reinforceTmpData[i].ToString();

            }
        }

    }


}
