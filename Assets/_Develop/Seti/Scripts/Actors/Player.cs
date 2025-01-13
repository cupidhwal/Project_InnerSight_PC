using UnityEngine;

namespace Seti
{
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
        protected float moveSpeed = 4f;
        [SerializeField]
        protected float dashSpeed = 30f;
        [SerializeField]
        protected float dashDuration = 0.15f;
        [SerializeField]
        protected float dashCooldown = 1f;

        [Header("View Type")]
        [SerializeField]
        private ViewType viewType;
        public ViewType View => viewType;
        #endregion

        // 속성
        #region Properties
        public float Speed_Move => moveSpeed;
        public float Dash_Speed => dashSpeed;
        public float Dash_Cooldown => dashCooldown;
        public float Dash_Duration => dashDuration;
        #endregion

        // 오버라이드
        #region Override
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_Player>();
        #endregion
    }
}