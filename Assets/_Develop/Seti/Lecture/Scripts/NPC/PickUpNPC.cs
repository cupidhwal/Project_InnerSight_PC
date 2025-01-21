using TMPro;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// NPC를 관리하는 클래스, 인터랙티브 기능 추가
    /// </summary>
    public class PickUpNPC : MonoBehaviour
    {
        // 필드
        #region Variables
        public NPC npc;

        protected Player player;
        protected float distance;

        public TextMeshProUGUI actionTextUI;
        public string actionText = "Pick Up";
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            // 참조
            player = FindAnyObjectByType<Player>();
        }
        #endregion

        // 메서드
        #region Methods
        protected virtual void ShowActionUI()
        {
            actionTextUI.gameObject.SetActive(true);
            actionTextUI.text = actionText + npc.name;
        }

        protected virtual void HiddenActionUI()
        {
            actionTextUI.gameObject.SetActive(false);
            actionTextUI.text = "";
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        protected virtual void OnMouseOver()
        {
            distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < 2f)
            {
                ShowActionUI();
            }
            else
            {
                HiddenActionUI();
            }
        }
        #endregion
    }
}