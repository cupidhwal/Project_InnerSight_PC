using Seti;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Noah
{
    public class PlayerStateManager : Singleton<PlayerStateManager>
    {
        public UpGradePlayerdata upGradePlayerdata;
        public StartData startPlayerData;

        public PlayerData playerData;
        public Gold upgradeGold;
        private List<float> dataList = new List<float>();
        private List<float> updateDataList = new List<float>();

        private List<int> updateCount = new List<int>();
        private List<int> updateGold = new List<int>();

        private GameObject player;

        Actor actor; 

        private void Start()
        {
            Init();
        }

        void Init()
        {
            playerData = new PlayerData();

            player = GameObject.FindWithTag("Player");
            actor = player.GetComponent<Actor>();

            playerData.ResetData();
            upGradePlayerdata.ResetData();
            upgradeGold.LoadData(playerData.LoadData);

            ResetData();

            actor.SetStats(playerData.Health, playerData.Attack, playerData.Defend,
                playerData.AttackSpeed, playerData.MoveSpeed);
        }

        void ResetData()
        {
            dataList.Add(playerData.Health);
            dataList.Add(playerData.Attack);
            dataList.Add(playerData.Defend);
            dataList.Add(playerData.MoveSpeed);
            dataList.Add(playerData.AttackSpeed);

            updateDataList.Add(upGradePlayerdata.hp_Up);
            updateDataList.Add(upGradePlayerdata.atk_Up);
            updateDataList.Add(upGradePlayerdata.def_Up);
            updateDataList.Add(upGradePlayerdata.moveSpeed_Up);
            updateDataList.Add(upGradePlayerdata.atkSpeed_Up);

            for (int i = 0; i < dataList.Count; i++)
            {
                updateCount.Add(i);
            }

            updateGold.Add(upgradeGold.hp_UpgradeGold);
            updateGold.Add(upgradeGold.atk_UpgradeGold);
            updateGold.Add(upgradeGold.def_UpgradeGold);
            updateGold.Add(upgradeGold.moveSpeed_UpgradeGold);
            updateGold.Add(upgradeGold.atkSpeed_UpgradeGold);
        }

        // 인덱스 번호로 플레이어데이터 찾기
        public float GetPlayerData(int _index)
        {
            return dataList[_index];
        }

        public int GetUpgradeCost(int _index)
        {
            return updateGold[_index];
        }

        public List<float> GetPlayerData()
        { 
            return dataList;
        }

        public List<int> GetUpgradeCost()
        {
            return updateGold;
        }

        public List<float> UpdatePlayerData()
        {
            return updateDataList;
        }

        public void UpdateStateData(List<Transform> dataTexts)
        {
            for(int i = 0; i < dataList.Count; i++)
            {
                dataList[i] = float.Parse(dataTexts[i].GetChild(0).GetComponent<TMP_Text>().text);
            }

            playerData.Health = dataList[0];
            playerData.Attack = dataList[1];
            playerData.Defend = dataList[2];
            playerData.MoveSpeed = dataList[3];
            playerData.AttackSpeed = dataList[4];

            actor.SetStats(playerData.Health, playerData.Attack, playerData.Defend, 
                playerData.AttackSpeed, playerData.MoveSpeed);
        }

        public int IncreaseGold(int _index)
        {
            int amount = updateCount[_index] * updateGold[_index];

            return amount;
        }
    }
}