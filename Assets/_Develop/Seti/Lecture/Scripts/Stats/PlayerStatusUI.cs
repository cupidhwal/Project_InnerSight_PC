using UnityEngine;
using TMPro;

namespace Seti
{
    public class PlayerStatusUI : MonoBehaviour
    {
        // 필드
        #region Variables
        public StatsObject stats;
        public InventoryObject equipment;

        public TextMeshProUGUI[] attributeTexts;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void OnEnable()
        {
            stats.OnChangedStats += OnChangedStats;

            if (equipment != null && stats != null)
            {
                foreach (var slot in equipment.Slots)
                {
                    slot.OnPreUpdate += OnRemoveItem;
                    slot.OnPostUpdate += OnEquipItem;
                }
            }

            UpdateAttributeTexts();
        }

        private void OnDisable()
        {
            stats.OnChangedStats -= OnChangedStats;
        }
        #endregion

        // 메서드
        #region Methods
        private void UpdateAttributeTexts()
        {
            attributeTexts[0].text = stats.GetModifiedValue(Attribute_Character.Agility).ToString();
            attributeTexts[1].text = stats.GetModifiedValue(Attribute_Character.Intellect).ToString();
            attributeTexts[2].text = stats.GetModifiedValue(Attribute_Character.Stamina).ToString();
            attributeTexts[3].text = stats.GetModifiedValue(Attribute_Character.Strength).ToString();
            attributeTexts[4].text = stats.GetModifiedValue(Attribute_Character.Health).ToString();
            attributeTexts[5].text = stats.GetModifiedValue(Attribute_Character.Mana).ToString();
        }

        private void OnChangedStats(StatsObject statsObject)
        {
            UpdateAttributeTexts();
        }

        void OnEquipItem(ItemSlot slot)
        {
            if (slot.ItemObject == null) return;

            Debug.Log($"OnEquipItem");

            if (slot.parent.type == InterfaceType.Equipment)
            {
                foreach (ItemSpec spec in slot.item.specs)
                {
                    foreach (var a in stats.attributes)
                    {
                        if (a.type == spec.stat)
                        {
                            a.value.AddModifier(spec);
                        }
                    }
                }
            }
        }

        void OnRemoveItem(ItemSlot slot)
        {
            if (slot.ItemObject == null) return;

            Debug.Log($"OnRemoveItem");

            if (slot.parent.type == InterfaceType.Equipment)
            {
                foreach (ItemSpec spec in slot.item.specs)
                {
                    foreach (var a in stats.attributes)
                    {
                        if (a.type == spec.stat)
                        {
                            a.value.RemoveModifier(spec);
                        }
                    }
                }
            }
        }
        #endregion
    }
}