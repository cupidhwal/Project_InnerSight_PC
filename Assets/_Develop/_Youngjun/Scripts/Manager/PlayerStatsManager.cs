using Seti;
using System;
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

        public InGameUI_RandomStats inGameUI_RandomStats;

        Actor actor; 

        private void Start()
        {
           
        }

        public void Init()
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

            if (inGameUI_RandomStats != null)
            {
                inGameUI_RandomStats.Init();
            }

            if (!SaveLoadManager.Instance.IsLoadData(SaveLoadManager.Instance.playerStatsSavePath))
            {
                SaveLoadManager.Instance.SaveAll();
            }

            //Invoke("Test", 2f);
        }

        void Test()
        {
            Damagable ec = player.GetComponent<Damagable>();

            if (ec != null)
            {
                // 데미지 데이터 가공 후 데미지 주기
                Damagable.DamageMessage data = new();
                data.amount = 90f;

                ec.TakeDamage(data);
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
                if (dataTexts[i].GetChild(0).GetComponent<TMP_Text>().text == "MAX" && i == 3)
                {
                    dataList[i] = inGameUI_PlayerState.maxData_Speed;
                }
                else if (dataTexts[i].GetChild(0).GetComponent<TMP_Text>().text == "MAX" && i == 4)
                {
                    dataList[i] = inGameUI_PlayerState.maxData_Atkspeed;
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
            Dictionary<string, (Action<float> statAction, int index)> reinDict = new()
            {
                { "체력",       (value => playerData.hp += value, 0) },
                { "공격력",     (value => playerData.atk += value, 1) },
                { "방어력",     (value => playerData.def += value, 2) },
                { "이동속도",   (value => playerData.moveSpeed += value, 3) },
                { "공격속도",   (value => playerData.atkSpeed += value, 4) }
            };

            if (reinDict.TryGetValue(_reinDic, out var reinData))
            {
                reinData.statAction(_reinData);
                inGameUI_RandomStats.ReinforceTmpData()[reinData.index] += _reinData;
                inGameUI_RandomStats.SetUIData();
            }
        }

        public void PlayerSetStat(string _reinDic, float _reinData)
        {
            switch (_reinDic)
            {
                case "체력":
                    player.GetComponent<Damagable>().HealCurrentHitPoint(_reinData);
                    break;
                case "공격력":
                    actor.Update_Attack(actor.Attack += _reinData);
                    break;
                case "방어력":
                    actor.Update_Defend(actor.Defend += _reinData);
                    break;
                case "이동속도":
                    actor.Update_Rate_Movement(actor.Rate_Movement += _reinData);
                    break;
                case "공격속도":
                    actor.Update_Rate_Attack(actor.Rate_Attack += _reinData);
                    break;
            }
        }

        public void SetReinforceData()
        {
            inGameUI_RandomStats.SetReinforceData();
        }


    }
}