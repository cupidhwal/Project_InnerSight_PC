using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Seti
{
    public class InventoryUI_Static : InventoryUI
    {
        // 필드
        #region Variables
        public GameObject[] staticSlots;
        #endregion

        // 오버라이드
        #region Override
        public override void CreateSlot()
        {
            invenSlots = new Dictionary<GameObject, ItemSlot>();
            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                GameObject go = staticSlots[i];

                // 생성된 슬롯 오브젝트의 이벤트 트리거에 이벤트 등록
                AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnter(go); });
                AddEvent(go, EventTriggerType.PointerExit, delegate { OnExit(go); });
                AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
                AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
                AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });

                inventoryObject.Slots[i].slotUI = go;
                invenSlots.Add(staticSlots[i], inventoryObject.Slots[i]);
            }
        }
        #endregion
    }
}