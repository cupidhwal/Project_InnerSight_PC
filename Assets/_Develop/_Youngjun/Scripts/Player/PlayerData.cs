using UnityEngine;
using System;
using Seti;

namespace Noah
{
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
    public class UpGradePlayerdata
    {
        public int hp_UpCount = 1;
        public int atk_UpCount = 1;
        public int def_UpCount = 1;
        public int moveSpeed_UpCount = 1;
        public int atkSpeed_UpCount = 1;

        public float hp_Up;
        public float atk_Up;
        public float def_Up;
        public float moveSpeed_Up;
        public float atkSpeed_Up;

        public void ResetData()
        {
            hp_Up = PlayerStateManager.Instance.upGradePlayerdata.hp_Up;
            atk_Up = PlayerStateManager.Instance.upGradePlayerdata.atk_Up;
            def_Up = PlayerStateManager.Instance.upGradePlayerdata.def_Up;
            moveSpeed_Up = PlayerStateManager.Instance.upGradePlayerdata.moveSpeed_Up;
            atkSpeed_Up = PlayerStateManager.Instance.upGradePlayerdata.atkSpeed_Up;         
        }
    }

    public class PlayerData
    {
        private float hp;
        private float atk;
        private float def;
        private float moveSpeed;
        private float atkSpeed;

        private bool loadData = false;

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

        public bool LoadData

        {
            get
            {
                return loadData;
            }
            set
            {
                loadData = value;
            }
        }

        public void ResetData()
        {
            if (!loadData)
            {
                hp = PlayerStateManager.Instance.startPlayerData.hp_Start;
                atk = PlayerStateManager.Instance.startPlayerData.atk_Start;
                def = PlayerStateManager.Instance.startPlayerData.def_Start;
                moveSpeed = PlayerStateManager.Instance.startPlayerData.moveSpeed_Start;
                atkSpeed = PlayerStateManager.Instance.startPlayerData.atkSpeed_Start;
            }
        }
    }
}