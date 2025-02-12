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

        Actor actor; 

        private void Start()
        {
            Init();
        }

        void Init()
        {
            playerData = SaveLoadManager.Instance.playerStats;
            upgradeCountData = SaveLoadManager.Instance.upgradeCount;

            player = GameObject.FindWithTag("Player");
            actor = player.GetComponent<Actor>();

            //playerData.ResetData();

            upgradeGold.ResetData();
            upgradeData.ResetData();

            inGameUI_PlayerState = FindAnyObjectByType<InGameUI_PlayerStats>();

            ResetData();

            actor.SetStats(playerData.Health, playerData.Attack, playerData.Defend,
                playerData.AttackSpeed, playerData.MoveSpeed);

            inGameUI_PlayerState.Init();

            if (!SaveLoadManager.Instance.isLoadData)
            {
                SaveLoadManager.Instance.SaveAll();
            }
            
        }

        #region ResetData
        void ResetData()
        {
            dataList.Add(playerData.Health);
            dataList.Add(playerData.Attack);
            dataList.Add(playerData.Defend);
            dataList.Add(playerData.MoveSpeed);
            dataList.Add(playerData.AttackSpeed);

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

            playerData.Health = dataList[0];
            playerData.Attack = dataList[1];
            playerData.Defend = dataList[2];
            playerData.MoveSpeed = dataList[3];
            playerData.AttackSpeed = dataList[4];

            upgradeCountData.hp_UpCount = upgradeCount[0];
            upgradeCountData.atk_UpCount = upgradeCount[1];
            upgradeCountData.def_UpCount = upgradeCount[2];
            upgradeCountData.moveSpeed_UpCount = upgradeCount[3];
            upgradeCountData.atkSpeed_UpCount = upgradeCount[4];

            actor.SetStats(playerData.Health, playerData.Attack, playerData.Defend,
                playerData.AttackSpeed, playerData.MoveSpeed);
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

      
    }
}