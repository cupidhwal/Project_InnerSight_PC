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
        [Header("Variables: Attack")]
        [SerializeField]
        private float attackProgressive = 2.5f;

        [Header("Variables: Dash")]
        [SerializeField]
        private float dashSpeed = 20f;
        [SerializeField]
        private float dashDuration = 0.667f;
        [SerializeField]
        private float dashCooldown = 1f;

        [Header("View Type")]
        [SerializeField]
        private ViewType viewType;
        public ViewType View => viewType;
        #endregion

        // 속성
        #region Properties
        // Player 전용
        public float AttackProgressive => attackProgressive;
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

        public void DashSpeed(float speed)
        {
            dashSpeed = speed;
        }
    }
}