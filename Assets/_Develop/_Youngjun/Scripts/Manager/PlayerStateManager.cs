using Seti;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Noah
{
    public class PlayerStateManager : Singleton<PlayerStateManager>
    {
        private InGameUI_PlayerState inGameUI_PlayerState;

        public UpGradePlayerdata upGradePlayerdata;
        public StartData startPlayerData;

        public PlayerData playerData;
        public Gold upgradeGold;
        public List<float> dataList = new List<float>();
        private List<float> updateDataList = new List<float>();

        private List<int> upgradeCount = new List<int>();
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

            //playerData.ResetData();
            upGradePlayerdata.ResetData();
            upgradeGold.LoadData(playerData.LoadData);

            inGameUI_PlayerState = FindAnyObjectByType<InGameUI_PlayerState>();

            ResetData();

            actor.SetStats(playerData.Health, playerData.Attack, playerData.Defend,
                playerData.AttackSpeed, playerData.MoveSpeed);

            inGameUI_PlayerState.Init();
        }

        #region ResetData
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

            upgradeCount.Add(upGradePlayerdata.hp_UpCount);
            upgradeCount.Add(upGradePlayerdata.atk_UpCount);
            upgradeCount.Add(upGradePlayerdata.def_UpCount);
            upgradeCount.Add(upGradePlayerdata.moveSpeed_UpCount);
            upgradeCount.Add(upGradePlayerdata.atkSpeed_UpCount);


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
                dataList[i] = float.Parse(dataTexts[i].GetChild(0).GetComponent<TMP_Text>().text);
                //updateGold[i] = int.Parse(dataTexts[i].GetChild(4).GetComponent<TMP_Text>().text);  
                upgradeCount[i] = _upgradeCount[i];
            }


            playerData.Health = dataList[0];
            playerData.Attack = dataList[1];
            playerData.Defend = dataList[2];
            playerData.MoveSpeed = dataList[3];
            playerData.AttackSpeed = dataList[4];

            upGradePlayerdata.hp_UpCount = upgradeCount[0];
            upGradePlayerdata.atk_UpCount = upgradeCount[1];
            upGradePlayerdata.def_UpCount = upgradeCount[2];
            upGradePlayerdata.moveSpeed_UpCount = upgradeCount[3];
            upGradePlayerdata.atkSpeed_UpCount = upgradeCount[4];

            //upgradeGold.hp_UpgradeGold = updateGold[i];
            //upgradeGold.atk_UpgradeGold = updateGold[i];
            //upgradeGold.def_UpgradeGold = updateGold[i];
            //upgradeGold.moveSpeed_UpgradeGold = updateGold[i];
            //upgradeGold.atkSpeed_UpgradeGold = updateGold[i];


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