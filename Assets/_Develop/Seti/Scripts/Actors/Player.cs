using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Player
    /// </summary>
    public class Player : Actor
    {
        // View type
        public enum ViewType
        {
            QuaterView,
            Follow_Person,
        }

        // 필드
        #region Variables
        [Header("Variables")]
        [SerializeField]
        protected float dashSpeed = 20f;
        [SerializeField]
        protected float dashDuration = 0.667f;
        [SerializeField]
        protected float dashCooldown = 1f;

        [Header("View Type")]
        [SerializeField]
        private ViewType viewType;
        public ViewType View => viewType;
        #endregion

        // 속성
        #region Properties
        // 스탯
        public float Dash_Speed => dashSpeed;
        public float Dash_Cooldown => dashCooldown;
        public float Dash_Duration => dashDuration;
        #endregion

        // 상호작용
        #region Interaction
        private NPC currentNPC;
        public NPC CurrentNPC => currentNPC;
        public void SetNPC(NPC npc) => currentNPC = npc;
        #endregion

        // 오버라이드
        #region Override
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_Player>();
        #endregion
    }
}