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

        public PlayerStatusUI playerStatusUI;
        public PlayerEquipmentUI playerEquipmentUI;
        public PlayerInventoryUI playerInventoryUI;
        public DialogUI dialogUI;

        public int itemID = 0;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 마우스 커서가 플레이 화면 밖으로 나가지 않도록 고정
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Toggle(playerInventoryUI.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Toggle(playerEquipmentUI.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                Toggle(playerStatusUI.gameObject);
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
        public void Toggle(GameObject go)
        {
            go.SetActive(!go.activeSelf);
        }

        public void OpenDialogueUI(int dialogIndex)
        {
            Toggle(dialogUI.gameObject);
            dialogUI.StartDialog(dialogIndex);
        }

        public void CloseDialogueUI()
        {
            Toggle(dialogUI.gameObject);
        }

        public void AddNewItem(int index)
        {
            ItemObject itemObject = database.itemObjects[index];
            Item newItem = itemObject.CreateItem();

            playerInventoryUI.inventoryObject.AddItem(newItem, 1);
        }
        #endregion
    }
}