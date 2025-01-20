using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 캐릭터 속성 값을 관리하는 클래스
    /// </summary>
    [Serializable]
    public class ModifiableInt
    {
        // 필드
        #region Variables
        [NonSerialized]
        private int baseValue;      // 기본 값
        [SerializeField]
        private int modifiedValue;  // 수정된 값, 최종 값

        // modifiedValue 값 변경 시 등록된 함수 호출
        private event Action<ModifiableInt> OnModifiedValue;

        // modifiedValue 값 계산 시 추가될 값들을 저장한 리스트
        private List<IModifier> modifiers = new();
        #endregion

        // 속성
        #region Properties
        public int BaseValue
        {
            get { return baseValue; }
            set
            {
                baseValue = value;
                UpdateModifiedValue();
            }
        }

        public int ModifiedValue
        {
            get { return modifiedValue; }
            set
            {
                modifiedValue = value;
            }
        }
        #endregion

        // 생성자 - 값 변경 시 호출할 함수를 매개변수로 받아 등록
        #region Constructor
        public ModifiableInt(Action<ModifiableInt> method = null)
        {
            ModifiedValue = baseValue;
            RegisterModEvent(method);
        }
        #endregion

        // 메서드
        #region Methods
        // 이벤트 구독
        public void RegisterModEvent(Action<ModifiableInt> method)
        {
            if (method != null)
            {
                OnModifiedValue += method;
            }
        }

        // 이벤트 구독 해제
        public void UnRegisterModEvent(Action<ModifiableInt> method)
        {
            if (method != null)
            {
                OnModifiedValue -= method;
            }
        }

        // 수정된 값 구하기
        private void UpdateModifiedValue()
        {
            int valutToAdd = 0;
            foreach (var modifier in modifiers)
            {
                modifier.AddValue(ref valutToAdd);
            }
            modifiedValue = baseValue + valutToAdd;

            OnModifiedValue?.Invoke(this);
        }

        public void AddModifier(IModifier modifier)
        {
            modifiers.Add(modifier);
            UpdateModifiedValue();
        }

        public void RemoveModifier(IModifier modifier)
        {
            modifiers.Remove(modifier);
            UpdateModifiedValue();
        }
        #endregion
    }
}