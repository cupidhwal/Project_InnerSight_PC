using System;
using System.Linq;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 아이템 슬롯을 취합하는 데이터 컨테이너
    /// </summary>
    [Serializable]
    public class Inventory
    {
        // 필드
        #region Variables
        public ItemSlot[] slots = new ItemSlot[16];
        #endregion

        // 메서드
        #region Methods
        // 인벤토리의 슬롯 초기화
        public void Clear()
        {
            foreach (var slot in slots)
            {
                // 빈 슬롯 만들기
                slot.UpdateSlot(new Item(), 0);
            }
        }

        // 아이템 찾기 : ID
        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id == id) != null;
        }

        // 아이템 찾기 : ItemObject
        public bool IsContain(ItemObject itemObject)
        {
            //return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
            return IsContain(itemObject.data.id);
        }
        #endregion
    }
}