using UnityEngine;
using System;
using Seti;

namespace Noah
{
    [Serializable]
    public class UpGradeData
    {
        public float hp_Up;
        public float atk_Up;
        public float def_Up;
        public float moveSpeed_Up;
        public float atkSpeed_Up;

        public void ResetData()
        {
            hp_Up = PlayerStatsManager.Instance.upgradeData.hp_Up;
            atk_Up = PlayerStatsManager.Instance.upgradeData.atk_Up;
            def_Up = PlayerStatsManager.Instance.upgradeData.def_Up;
            moveSpeed_Up = PlayerStatsManager.Instance.upgradeData.moveSpeed_Up;
            atkSpeed_Up = PlayerStatsManager.Instance.upgradeData.atkSpeed_Up;
        }
    }

    [Serializable]
    public class StartData
    {
        public float hp_Start;
        public float atk_Start;
        public float def_Start;
        public float moveSpeed_Start;
        public float atkSpeed_Start;
    }

    [Serializable]
    public class UpGradeCountData
    {
        public int hp_UpCount = 1;
        public int atk_UpCount = 1;
        public int def_UpCount = 1;
        public int moveSpeed_UpCount = 1;
        public int atkSpeed_UpCount = 1;    
    }

    [Serializable]
    public class PlayerData
    {
        public float hp;
        public float atk;
        public float def;
        public float moveSpeed;
        public float atkSpeed;

        public float Health
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
            }
        }

        public float Attack
        {
            get
            {
                return atk;
            }
            set
            {
                atk = value;
            }
        }

        public float Defend
        {
            get
            {
                return def;
            }
            set
            {
                def = value;
            }
        }
        public float MoveSpeed
        {
            get
            {
                return moveSpeed;
            }
            set
            {
                moveSpeed = value;
            }
        }
        public float AttackSpeed
        {
            get
            {
                return atkSpeed;
            }
            set
            {
                atkSpeed = value;
            }
        }

        //public PlayerData(float _hp, float _atk, float _def, float _moveSpeed, float _atkSpeed)
        //{
        //    hp = _hp;
        //    atk = _atk;
        //    def = _def;
        //    moveSpeed = _moveSpeed;
        //    atkSpeed = _atkSpeed;
        //}

        public void ResetData()
        {
            if (!SaveLoadManager.Instance.isLoadData)
            {
                hp = PlayerStatsManager.Instance.startPlayerData.hp_Start;
                atk = PlayerStatsManager.Instance.startPlayerData.atk_Start;
                def = PlayerStatsManager.Instance.startPlayerData.def_Start;
                moveSpeed = PlayerStatsManager.Instance.startPlayerData.moveSpeed_Start;
                atkSpeed = PlayerStatsManager.Instance.startPlayerData.atkSpeed_Start;
            }
            else
            {
                // 저장된 데이터가 있으면 적용
                PlayerData loadedStats = SaveLoadManager.Instance.playerStats;
                
                hp = loadedStats.hp;
                atk = loadedStats.atk;
                def = loadedStats.def;
                moveSpeed = loadedStats.moveSpeed;
                atkSpeed = loadedStats.atkSpeed;
            }
        }

    }
}