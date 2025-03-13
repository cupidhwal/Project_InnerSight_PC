using JungBin;
using Noah;
using UnityEngine;

namespace Seti
{
    public enum NPC_Type
    {
        Storyteller,
        Enhance,
        Trinkets,
    }

    public class NPC : Actor
    {
        // 필드
        #region Variables
        [SerializeField]
        private NPC_Type type;
        private GameObject targetUI;
        private bool isOpenUI = false;
        #endregion

        // 속성
        public NPC_Type Type => type;

        // 오버라이드
        #region Override
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_NPC>();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            SetUI();
        }
        #endregion

        // 메서드
        #region Methods
        public void SetUI()
        {
            switch(type)
            {
                case NPC_Type.Trinkets:
                    targetUI = Noah.UIManager.Instance.trinketsUI;
                    break;

                case NPC_Type.Enhance:
                    targetUI = Noah.UIManager.Instance.playerStateUI;
                    break;
            }
        }
        public void Switch_TradeUI()
        {
            switch (type)
            {
                case NPC_Type.Trinkets:
                    if (RelicManager.Instance.GetRelics().Count > 0)
                    {
                        targetUI.SetActive(isOpenUI = !isOpenUI);
                    }
                    break;

                case NPC_Type.Enhance:
                    Noah.UIManager.Instance.ActivePlayerStateUI();
                    break;
            }
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<Player>(out var player))
                player.SetNPC(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent<Player>(out var player))
            {
                player.SetNPC(null);

                if (targetUI != null && targetUI.activeSelf)
                    Noah.UIManager.Instance.ActivePlayerStateUI();
            }
        }
        #endregion
    }
}