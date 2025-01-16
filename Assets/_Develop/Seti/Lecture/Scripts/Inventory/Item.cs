using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Item
    /// ItemSpec
    /// </summary>
    [System.Serializable]
    public class Item
    {
        // 필드
        #region Variables
        public int id;
        public string name;

        public ItemSpec[] specs;
        #endregion

        // 생성자
        #region Constructor
        public Item()
        {
            id = -1;
            name = "";
        }

        // 매개변수로 ItemObject를 받아서 아이템 데이터 세팅
        public Item(ItemObject itemObject)
        {
            this.name = itemObject.name;
            this.id = itemObject.data.id;

            specs = new ItemSpec[itemObject.data.specs.Length];
            for (int i = 0; i < specs.Length; i++)
            {
                specs[i] = new ItemSpec(itemObject.data.specs[i].Min, itemObject.data.specs[i].Max)
                {
                    stat = itemObject.data.specs[i].stat,
                };
            }
        }
        #endregion
    }
}