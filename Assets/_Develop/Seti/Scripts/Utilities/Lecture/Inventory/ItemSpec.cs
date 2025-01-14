using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 아이템의 스펙 정의
    /// </summary>
    [System.Serializable]
    public class ItemSpec
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

        // 메서드
        #region Methods
        public void GenerateValue()
        {
            value = Random.Range(min, max);
        }
        #endregion
    }
}