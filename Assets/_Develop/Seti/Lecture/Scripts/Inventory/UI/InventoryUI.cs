using NUnit.Framework.Constraints;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Seti
{
    /// <summary>
    /// Inventory UI를 제어하는 추상 클래스
    /// </summary>
    [RequireComponent(typeof(EventTrigger))]
    public abstract class InventoryUI : MonoBehaviour
    {
        // 필드
        #region Variables
        public InventoryObject inventoryObject;

        public Dictionary<GameObject, ItemSlot> invenSlots = new();
        #endregion

        // 추상화
        #region Abstract
        public abstract void CreateSlot();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // UI 슬롯 갱신
            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
            }
        }

        private void Awake()
        {
            CreateSlot();

            // InventoryObject Slots 설정
            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                inventoryObject.Slots[i].parent = inventoryObject;

                // 아이템 내용 변경 시 호출되는 UI 함수 등록
                inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
            }

            // 이벤트 트리거 이벤트 등록
            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        }
        #endregion

        // 메서드
        #region Methods
        private void OnPostUpdate(ItemSlot itemSlot)
        {
            // 아이템 슬롯 체크
            if (itemSlot == null || itemSlot.slotUI == null)
            {
                return;
            }

            Image itemImage = itemSlot.slotUI.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI itemCount = itemSlot.slotUI.transform.GetComponentInChildren<TextMeshProUGUI>();
            itemImage.sprite = itemSlot.item.id < 0 ? 
                               null : 
                               itemSlot.ItemObject.icon;
            itemImage.color = itemSlot.item.id < 0 ? 
                              new Color(1, 1, 1, 0) : 
                              new Color(1, 1, 1, 1);
            itemCount.text = itemSlot.item.id < 0 ?
                             string.Empty :
                             (itemSlot.amount == 1 ? string.Empty : itemSlot.amount.ToString());
        }

        // 이벤트 트리거 등록
        protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            if (go.TryGetComponent<EventTrigger>(out var trigger))
            {
                EventTrigger.Entry eventTrigger = new() { eventID = type };
                eventTrigger.callback.AddListener(action);
                trigger.triggers.Add(eventTrigger);
            }
            else
            {
                Debug.LogWarning("No EventTrigger component found!");
                return;
            }
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        // 게임 오브젝트에 마우스가 들어갈 때 호출
        private void OnEnterInterface(GameObject go)
        {
            MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
        }

        // 게임 오브젝트에 마우스가 나갈 때 호출
        private void OnExitInterface(GameObject go)
        {
            MouseData.interfaceMouseIsOver = null;
        }

        // 슬롯 오브젝트에 마우스가 들어갈 때 호출
        public void OnEnter(GameObject go)
        {
            MouseData.slotHoveredOver = go;
            MouseData.interfaceMouseIsOver = GetComponentInParent<InventoryUI>();
        }

        // 슬롯 오브젝트에서 마우스가 나갈 때 호출
        public void OnExit(GameObject go)
        {
            MouseData.slotHoveredOver = null;
        }

        // 마우스 드래그 시 마우스 포인터에 달고 다니는 이미지 오브젝트 생성
        private GameObject CreateDragImage(GameObject go)
        {
            if (invenSlots[go].item.id < 0)
                return null;

            GameObject dragImage = new();

            RectTransform rect = dragImage.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(50, 50);
            dragImage.transform.SetParent(transform.parent);

            Image image = dragImage.AddComponent<Image>();
            image.sprite = invenSlots[go].ItemObject.icon;
            image.raycastTarget = false;

            dragImage.name = "Drag Image";
            return dragImage;
        }

        // 슬롯 오브젝트에 마우스를 드래그할 때 호출
        public void OnStartDrag(GameObject go)
        {
            MouseData.tempItemBeginDragged = CreateDragImage(go);
        }

        // 마우스 드래그 시 마우스 포인터에 달고 다니는 이미지 위치 설정
        public void OnDrag(GameObject go)
        {
            if (MouseData.tempItemBeginDragged == null)
                return;

            MouseData.tempItemBeginDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }

        // 마우스 드래그 종료
        public void OnEndDrag(GameObject go)
        {
            Destroy(MouseData.tempItemBeginDragged);

            // 마우스의 위치가 인벤토리 UI 밖에 있을 경우
            if (MouseData.interfaceMouseIsOver == null)
            {
                // 아이템 버리기
                invenSlots[go].RemoveItem();
            }
            // 마우스의 위치가 슬롯 위에 있을 경우
            else if (MouseData.slotHoveredOver)
            {
                // 마우스가 위치한 게임 오브젝트의 슬롯
                ItemSlot secondSlot = MouseData.interfaceMouseIsOver.invenSlots[MouseData.slotHoveredOver];

                // 아이템 바꾸기
                inventoryObject.SwapItems(invenSlots[go], secondSlot);
            }
        }
        #endregion
    }
}