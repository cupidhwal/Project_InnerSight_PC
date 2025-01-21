using System;
using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Stats", menuName = "Status System/New Character Status")]
    public class StatsObject : ScriptableObject
    {
        // 필드
        #region Variables
        public Attribute[] attributes;

        public int level;
        public int exp;

        // 스탯 변경 시 등록된 메서드 호출
        public Action<StatsObject> OnChangedStats;

        // 초기화 확인
        [NonSerialized]
        private bool isInitialized = false;
        #endregion

        // 속성
        #region Properties
        public int Health { get; set; }
        public int Mana { get; set; }
        public float HealthPercentage
        {
            get
            {
                int health = Health;
                int maxHealth = health;

                foreach (Attribute a in attributes)
                {
                    if (a.type == Attribute_Character.Health)
                    {
                        maxHealth = a.value.ModifiedValue;
                    }
                }

                return (maxHealth > 0) ? ((float)health / (float)maxHealth) : 0f;
            }
        }
        public float ManaPercentage
        {
            get
            {
                int mana = Mana;
                int maxMana = mana;

                foreach (Attribute a in attributes)
                {
                    if (a.type == Attribute_Character.Mana)
                    {
                        maxMana = a.value.ModifiedValue;
                    }
                }

                return (maxMana > 0) ? ((float)mana / (float)maxMana) : 0f;
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void OnEnable()
        {
            InitializeAttributes();
        }
        #endregion

        // 메서드
        #region Methods
        void InitializeAttributes()
        {
            if (isInitialized) return;

            level = 1;
            exp = 0;

            foreach (Attribute a in attributes)
            {
                // attributes의 value 객체 생성
                a.value = new(OnModifiedValue);
            }

            SetBaseValue(Attribute_Character.Agility, 100);
            SetBaseValue(Attribute_Character.Intellect, 100);
            SetBaseValue(Attribute_Character.Stamina, 100);
            SetBaseValue(Attribute_Character.Strength, 100);
            SetBaseValue(Attribute_Character.Health, 100);
            SetBaseValue(Attribute_Character.Mana, 100);

            // Current Health, Mana 초기화
            Health = GetModifiedValue(Attribute_Character.Health);
            Mana = GetModifiedValue(Attribute_Character.Mana);

            isInitialized = true;
            //Debug.Log("스탯 초기화 완료!");
        }

        // 속성값 초기화
        void SetBaseValue(Attribute_Character type, int value)
        {
            foreach (Attribute a in attributes)
            {
                if (a.type == type)
                {
                    a.value.BaseValue = value;
                }
            }
        }

        // 기본 속성값 가져오기
        int GetBaseValue(Attribute_Character type)
        {
            foreach (Attribute a in attributes)
            {
                if (a.type == type)
                {
                    return a.value.BaseValue;
                }
            }

            return -1;
        }

        // 최종 속성값 가져오기
        public int GetModifiedValue(Attribute_Character type)
        {
            foreach (Attribute a in attributes)
            {
                if (a.type == type)
                {
                    return a.value.ModifiedValue;
                }
            }

            return -1;
        }

        // 모든 attribute의 value 값이 변경되면 호출되는 함수
        void OnModifiedValue(ModifiableInt value)
        {
            // 스탯 변경 시 등록된 함수 호출
            OnChangedStats?.Invoke(this);
        }
        #endregion
    }
}