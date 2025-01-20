using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        // 필드
        #region Variables
        public ItemDatabase database;
        public InventoryUI_Static inventoryUI_Static;
        public InventoryUI_Dynamic inventoryUI_Dynamic;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryUI_Dynamic.gameObject.SetActive(!inventoryUI_Dynamic.gameObject.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                inventoryUI_Static.gameObject.SetActive(!inventoryUI_Static.gameObject.activeSelf);

                // 커서
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddNewItem(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AddNewItem(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AddNewItem(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AddNewItem(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                AddNewItem(4);
            }
        }
        #endregion

        // 메서드
        #region Methods
        public void AddNewItem(int index)
        {
            ItemObject itemObject = database.itemObjects[index];
            Item newItem = itemObject.CreateItem();

            inventoryUI_Dynamic.inventoryObject.AddItem(newItem, 1);
        }
        #endregion
    }
}