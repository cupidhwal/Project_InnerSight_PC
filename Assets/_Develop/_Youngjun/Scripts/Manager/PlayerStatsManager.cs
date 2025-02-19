using Seti;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Noah
{
    public class PlayerStatsManager : Singleton<PlayerStatsManager>
    {
        private InGameUI_PlayerStats inGameUI_PlayerState;

        public UpGradeData upgradeData;
        public StartData startPlayerData;
  
        public Gold upgradeGold;
        public List<float> dataList = new List<float>();
        private List<float> updateDataList = new List<float>();

        private List<int> upgradeCount = new List<int>();
        private List<int> updateGold = new List<int>();

        private GameObject player;

        PlayerData playerData;
        UpGradeCountData upgradeCountData;
<<<<<<< HEAD
<<<<<<< HEAD
        public InGameUI_RandomStats inGameUI_RandomStats;
=======
>>>>>>> parent of 3ee181fd (Merge remote-tracking branch 'origin/youngjun' into develop)
=======
        InGameUI_RandomStats inGameUI_RandomStats;
>>>>>>> 3ee181fd1f14ad066db3d590fe1a3c0fa881e0bb

        Actor actor; 

        private void Start()
        {
            Init();
        }

        void Init()
        {
            playerData = SaveLoadManager.Instance.playerStats;
            upgradeCountData = SaveLoadManager.Instance.upgradeCount;
            inGameUI_RandomStats = FindAnyObjectByType<InGameUI_RandomStats>();

            player = GameObject.FindWithTag("Player");
            actor = player.GetComponent<Actor>();

            //playerData.ResetData();

            upgradeGold.ResetData();
            upgradeData.ResetData();

            inGameUI_PlayerState = FindAnyObjectByType<InGameUI_PlayerStats>();

            ResetData();
            SetPlayerStat();

            inGameUI_PlayerState.Init();
            inGameUI_RandomStats.Init();

            if (!SaveLoadManager.Instance.isLoadData)
            {
                SaveLoadManager.Instance.SaveAll();
            }        
        }

        #region ResetData
        void ResetData()
        {
            dataList.Add(playerData.hp);
            dataList.Add(playerData.atk);
            dataList.Add(playerData.def);
            dataList.Add(playerData.moveSpeed);
            dataList.Add(playerData.atkSpeed);

            updateDataList.Add(upgradeData.hp_Up);
            updateDataList.Add(upgradeData.atk_Up);
            updateDataList.Add(upgradeData.def_Up);
            updateDataList.Add(upgradeData.moveSpeed_Up);
            updateDataList.Add(upgradeData.atkSpeed_Up);

            upgradeCount.Add(upgradeCountData.hp_UpCount);
            upgradeCount.Add(upgradeCountData.atk_UpCount);
            upgradeCount.Add(upgradeCountData.def_UpCount);
            upgradeCount.Add(upgradeCountData.moveSpeed_UpCount);
            upgradeCount.Add(upgradeCountData.atkSpeed_UpCount);


            updateGold.Add(upgradeGold.hp_UpgradeGold);
            updateGold.Add(upgradeGold.atk_UpgradeGold);
            updateGold.Add(upgradeGold.def_UpgradeGold);
            updateGold.Add(upgradeGold.moveSpeed_UpgradeGold);
            updateGold.Add(upgradeGold.atkSpeed_UpgradeGold);
        }
        #endregion

        #region 업그레이드 데이터 동기화
        public void UpdateStateData(List<Transform> dataTexts, int[] _upgradeCount)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataTexts[i].GetChild(0).GetComponent<TMP_Text>().text == "MAX" &&
                    (i == 3 || i == 4))
                {
                    dataList[i] = inGameUI_PlayerState.maxData_Speed;
                }
                else
                {
                    dataList[i] = float.Parse(dataTexts[i].GetChild(0).GetComponent<TMP_Text>().text);
                }

                
                //updateGold[i] = int.Parse(dataTexts[i].GetChild(4).GetComponent<TMP_Text>().text);  
                upgradeCount[i] = _upgradeCount[i];
            }

            playerData.hp = dataList[0];
            playerData.atk = dataList[1];
            playerData.def = dataList[2];
            playerData.moveSpeed = dataList[3];
            playerData.atkSpeed = dataList[4];

            upgradeCountData.hp_UpCount = upgradeCount[0];
            upgradeCountData.atk_UpCount = upgradeCount[1];
            upgradeCountData.def_UpCount = upgradeCount[2];
            upgradeCountData.moveSpeed_UpCount = upgradeCount[3];
            upgradeCountData.atkSpeed_UpCount = upgradeCount[4];

            actor.SetStats(playerData.hp, playerData.atk, playerData.def,
                playerData.atkSpeed, playerData.moveSpeed);
        }
        #endregion

        // 인덱스 번호로 플레이어데이터 찾기
        public float GetPlayerData(int _index)
        {
            return dataList[_index];
        }
        public List<float> GetPlayerData()
        {
            return dataList;
        }

        // 인덱스 번호로 업그레이드 코스트 찾기
        public int GetUpgradeCost(int _index)
        {
            return updateGold[_index];
        }

        public List<int> GetUpgradeCost()
        {
            return updateGold;
        }

        public List<float> UpdatePlayerData()
        {
            return updateDataList;
        }

        public List<int> UpgardeCount()
        {
            return upgradeCount;
        }


        public void SetPlayerStat()
        {
            actor.SetStats(playerData.hp, playerData.atk, playerData.def,
                playerData.atkSpeed, playerData.moveSpeed);

        }

        public void PlayerStatReinforce(string _reinDic, float _reinData)
        {
            switch (_reinDic)
            {
                case "체력":
                    playerData.hp += _reinData;
                    break;
                case "공격력":
                    playerData.atk += _reinData;
                    break;
                case "방어력":
                    playerData.def += _reinData;
                    break;
                case "이동속도":
                    playerData.moveSpeed += _reinData;
                    break;
                case "공격속도":
                    playerData.atkSpeed += _reinData;
                    break;
            }
        }

    }
}