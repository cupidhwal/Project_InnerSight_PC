using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 아이템 기본 정보를 담는 컨테이너
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item/New Item")]
    public class ItemObject : ScriptableObject
    {
        // 필드
        #region Variables
        public Item data = new();

        public ItemType type;
        public bool stackable;  // 슬롯에 누적 여부
        public Sprite icon;
        public GameObject modelPrefab;

        [TextArea(15, 20)]
        public string description;
        #endregion
    }
}