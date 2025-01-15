using UnityEngine;
using System;

namespace Noah
{
    [Serializable]
    public class StartData
    {
        public float atk_Start;
        public float hp_Start;
        public float def_Start;
        public float moveSpeed_Start;
        public float atkSpeed_Start;
    }

    [Serializable]
    public class UpGradePlayerdata
    {
        public float atk_Up;
        public float hp_Up;
        public float def_Up;
        public float moveSpeed_Up;
        public float atkSpeed_Up;
    }

    public class PlayerData
    {
        private float atk;
        private float hp;
        private float def;
        private float moveSpeed;
        private float atkSpeed;

        private bool loadData = false;

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

        public void ResetData()
        {
            if (!loadData)
            {
                atk = PlayerStateManager.Instance.startPlayerData.atk_Start;
                hp = PlayerStateManager.Instance.startPlayerData.atk_Start;
                def = PlayerStateManager.Instance.startPlayerData.atk_Start;
                moveSpeed = PlayerStateManager.Instance.startPlayerData.atk_Start;
                atkSpeed = PlayerStateManager.Instance.startPlayerData.atk_Start;
            }

        }

    }
}