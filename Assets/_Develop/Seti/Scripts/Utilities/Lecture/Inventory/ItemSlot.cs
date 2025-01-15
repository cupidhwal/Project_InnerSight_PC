using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ItemSlot
    {
        // 필드
        #region Variables
        public Item item;
        public int amount;

        public ItemType[] AllowedItems = new ItemType[0];   // 슬롯에 넣을 수 있는 아이템 타입 설정

        [NonSerialized]
        public InventoryObject parent;

        [NonSerialized]
        public GameObject slotUI;   // 슬롯이 적용되는 UI 오브젝트

        [NonSerialized]
        public Action<ItemSlot> OnPreUpdate;    // 슬롯에 아이템 내용이 적용되기 이전에 등록된 함수 호출
        [NonSerialized]
        public Action<ItemSlot> OnPostUpdate;   // 슬롯에 아이템 내용이 적용된 이후에 등록된 함수 호출
        #endregion

        // 속성
        #region Properties
        public ItemObject ItemObject
        {
            get { return item.id >= 0 ? parent.database.itemObjects[item.id] : null; }
        }
        #endregion

        // 생성자
        #region Constructor
        public ItemSlot()
        {
            // 빈 슬롯(아이템이 없는 슬롯)
            UpdateSlot(new Item(), 0);
        }

        public ItemSlot(Item item, int amount)
        {
            // 매개변수로 들어온 아이템을 가진 슬롯 생성
            UpdateSlot(item, amount);
        }
        #endregion

        // 메서드
        #region Methods
        // 슬롯 업데이트
        public void UpdateSlot(Item item, int amount)
        {
            // 빈 슬롯 체크
            if (amount == 0)
            {
                item = new Item();
            }

            OnPreUpdate?.Invoke(this);
            this.item = item;
            this.amount = amount;
            OnPostUpdate?.Invoke(this);
        }

        // 아이템 수량 추가
        public void AddItemAmount(int value)
        {
            int addValue = amount + value;
            UpdateSlot(item, addValue);
        }

        // 슬롯에서 아이템 삭제
        public void RemoveItem()
        {
            UpdateSlot(new Item(), 0);
        }

        // 슬롯에 아이템 할당 가능 여부 판단
        public bool CanPlaceInSlot(ItemObject itemObject)
        {
            if (AllowedItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
            {
                return true;
            }

            // AllowedItem 체크
            foreach (var itemType in AllowedItems)
            {
                if (itemType == itemObject.type)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}