using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Inventory 데이터 컨테이너를 가지고 있는 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory/New Inventory")]
    public class InventoryObject : ScriptableObject
    {
        // 필드
        #region Variables
        public ItemDatabase database;       // 아이템 정보를 가지고 있는 스크립터블 오브젝트
        public InterfaceType type;          // 인벤토리 타입

        public Inventory container = new();

        public Action<ItemObject> OnUseItem;// 아이템 사용 시 등록된 메서드 호출
        #endregion

        // 속성
        #region Properties
        // 인벤토리 슬롯
        public ItemSlot[] Slots => container.slots;

        // 현재 빈 슬롯 갯수
        public int EmptySlotCount
        {
            get
            {
                int count = 0;
                foreach (var slot in Slots)
                {
                    if (slot.item.id < 0)
                        count++;
                }
                return count;
            }
        }
        #endregion

        // 메서드
        #region Methods
        // 인벤토리에 아이템 추가
        public bool AddItem(Item item, int amount)
        {
            ItemSlot slot = FindItemInInventory(item);
            if (!database.itemObjects[item.id].stackable || slot == null)
            {
                // 인벤토리가 가득 찬 경우
                if (EmptySlotCount == 0)
                {
                    Debug.Log("Inventory is Full");
                    return false;
                }

                // 빈 슬롯에 아이템 추가
                ItemSlot emptySlot = GetEmptySlot();
                emptySlot.UpdateSlot(item, amount);
            }
            else
            {
                // 아이템을 가진 슬롯에 amount 누적
                slot.AddItemAmount(amount);
            }

            return true;
        }

        // 매개변수로 들어온 아이템을 가진 슬롯 찾기
        public ItemSlot FindItemInInventory(Item item)
        {
            return Slots.FirstOrDefault(i => i.item.id == item.id);
        }

        // 매개변수로 들어온 아이템 오브젝트가 인벤토리에 있는지 여부
        public bool IsContainItem(ItemObject itemObject)
        {
            return Slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;
        }

        // 빈 슬롯 찾기
        public ItemSlot GetEmptySlot()
        {
            return Slots.FirstOrDefault(i => i.item.id < 0);
        }

        // 아이템 바꾸기
        public void SwapItems(ItemSlot itemA, ItemSlot itemB)
        {
            // 같은 슬롯일 경우 실행 중지
            if (itemA == itemB)
            {
                return;
            }

            if (itemB.CanPlaceInSlot(itemA.ItemObject) && itemA.CanPlaceInSlot(itemB.ItemObject))
            {
                ItemSlot temp = new(itemB.item, itemB.amount);
                itemB.UpdateSlot(itemA.item, itemA.amount);
                itemA.UpdateSlot(temp.item, temp.amount);
            }
        }

        // 아이템 사용하기
        public void UseItem(ItemSlot useSlot)
        {
            if (useSlot.ItemObject == null || useSlot.item.id < 0 || useSlot.amount <= 0)
            {
                return;
            }

            ItemObject itemObject = useSlot.ItemObject;
            useSlot.UpdateSlot(useSlot.item, useSlot.amount - 1);

            OnUseItem?.Invoke(itemObject);
        }

        // 인벤토리 데이터 저장하기/불러오기
        #region Save/Load Methods
        public string savePath = "/Inventory.json";

        [ContextMenu("Save")]
        public void Save()
        {
            string path = Application.persistentDataPath + savePath;

            BinaryFormatter bf = new();                                     // 데이터 이진화 준비
            FileStream fs = new(path, FileMode.Create, FileAccess.Write);   // 저장할 파일에 접근
            string saveData = JsonUtility.ToJson(container);                // 저장할 데이터를 준비
            bf.Serialize(fs, saveData);                                     // 데이터 저장
            fs.Close();                                                     // 파일 닫기
        }

        [ContextMenu("Load")]
        public void Load()
        {
            string path = Application.persistentDataPath + savePath;
            
            if (File.Exists(path))
            {
                BinaryFormatter bf = new();
                FileStream fs = new(path, FileMode.Open, FileAccess.Read);
                string loadData = bf.Deserialize(fs).ToString();
                JsonUtility.FromJsonOverwrite(loadData, container);
                fs.Close();
            }
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            container.Clear();
        }
        #endregion
        #endregion
    }
}