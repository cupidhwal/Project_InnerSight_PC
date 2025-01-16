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
        public InventoryUI_Dynamic inventoryUI_Player;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryUI_Player.gameObject.SetActive(!inventoryUI_Player.gameObject.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                AddNewItem();
            }
        }
        #endregion

        // 메서드
        #region Methods
        public void AddNewItem()
        {
            ItemObject itemObject = database.itemObjects[1];
            Item newItem = itemObject.CreateItem();

            inventoryUI_Player.inventoryObject.AddItem(newItem, 1);
        }
        #endregion
    }
}