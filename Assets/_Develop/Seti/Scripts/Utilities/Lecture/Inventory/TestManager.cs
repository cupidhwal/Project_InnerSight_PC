using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Test - Inventory
    /// </summary>
    public class TestManager : MonoBehaviour
    {
        public ItemDatabase database;
        public InventoryObject inventoryObject;

        private void Start()
        {
            Item newItem = database.itemObjects[0].CreateItem();
            inventoryObject.AddItem(newItem, 1);
        }
    }
}