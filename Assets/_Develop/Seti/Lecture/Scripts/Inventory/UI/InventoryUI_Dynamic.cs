using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Seti
{
    public class InventoryUI_Dynamic : InventoryUI
    {
        // 필드
        #region Variables
        public Transform slotsParent;
        public GameObject slotPrefab;
        #endregion

        // 오버라이드
        #region Override
        public override void CreateSlot()
        {
            //invenSlots = new();

            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                GameObject go = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, slotsParent);

                // 생성된 슬롯 오브젝트의 이벤트 트리거에 이벤트 등록
                AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnter(go); });
                AddEvent(go, EventTriggerType.PointerExit, delegate { OnExit(go); });
                AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
                AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
                AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });

                // UI 세팅은 여기에서
                inventoryObject.Slots[i].slotUI = go;
                invenSlots.Add(go, inventoryObject.Slots[i]);
                go.name += ": " + i.ToString();
            }
        }
        #endregion
    }
}