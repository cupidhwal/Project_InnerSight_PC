using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public class PlayerStateManager : Singleton<PlayerStateManager>
    {
        public UpGradePlayerdata upGradePlayerdata;
        public StartData startPlayerData;

        private PlayerData playerData;
        private List<float> dataList = new List<float>();

        private void Start()
        {
            Init();
        }

        void Init()
        {
            playerData = new PlayerData();

            playerData.ResetData();

            dataList.Add(playerData.Attack);
            dataList.Add(playerData.Health);
            dataList.Add(playerData.Defend);
            dataList.Add(playerData.MoveSpeed);
            dataList.Add(playerData.AttackSpeed);
        }

        public List<float> GetPlayerData()
        { 
            return dataList;
        }

        public void UpdatePlayerData()
        { 
            
        }

        // UI 확인용
        public void AddState()
        {

        }

        // UI 확인용
        public void RemoveState()
        {

        }

        
    }
}