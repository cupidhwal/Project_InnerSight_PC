using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 아이템의 스펙 정의
    /// </summary>
    [Serializable]
    public class ItemSpec : IModifier
    {
        // 필드
        #region Variables
        public CharAttribute stat;
        public int value;

        [SerializeField] private int min;
        [SerializeField] private int max;
        #endregion

        // 속성
        #region Properties
        public int Min => min;
        public int Max => max;
        #endregion

        // 생성자
        #region Constructor
        public ItemSpec(int min, int max)
        {
            this.min = min;
            this.max = max;
            GenerateValue();
        }
        #endregion

        // 인터페이스
        #region Interface
        // 매개변수로 입력받은 변수에 value 값을 누적한다
        public void AddValue(ref int baseValue)
        {
            baseValue += value;
        }
        #endregion

        // 메서드
        #region Methods
        public void GenerateValue()
        {
            value = UnityEngine.Random.Range(min, max);
        }
        #endregion
    }
}