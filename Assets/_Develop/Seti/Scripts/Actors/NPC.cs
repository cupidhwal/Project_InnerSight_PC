using UnityEngine;

namespace Seti
{
    public class NPC : Actor
    {
        // 필드
        #region Variables
        private Player player;
        [SerializeField]
        private GameObject targetUI;
        #endregion

        // 오버라이드
        #region Override
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_NPC>();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Start()
        {
            base.Start();
            targetUI = Noah.UIManager.Instance.playerStateUI;
        }
        #endregion

        // 메서드
        #region Methods
        public void Switch_TradeUI() => Noah.UIManager.Instance.ActivePlayerStateUI();
        #endregion

        // 이벤트 메서드
        #region Event Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<Player>(out var player))
            {
                this.player = player;
                player.SetNPC(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (player != null)
                player.SetNPC(null);

            if (targetUI != null && targetUI.activeSelf)
                Noah.UIManager.Instance.ActivePlayerStateUI();
        }
        #endregion
    }
}