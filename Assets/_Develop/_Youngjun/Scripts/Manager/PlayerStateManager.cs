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
        public List<float> dataList = new List<float>();
        public List<float> updateDataList = new List<float>();

        private void Start()
        {
            Init();
        }

        void Init()
        {
            playerData = new PlayerData();

            playerData.ResetData();
            upGradePlayerdata.ResetData();

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
        }

        public List<float> GetPlayerData()
        { 
            return dataList;
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
                Debug.Log();
            }
        }
    }
}