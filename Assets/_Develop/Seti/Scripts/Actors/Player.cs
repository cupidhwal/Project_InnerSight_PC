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
        [Header("View Type")]
        [SerializeField]
        private ViewType viewType;
        public ViewType View => viewType;
        #endregion

        // 오버라이드
        #region Override
        protected override State_Actor CreateState() => gameObject.AddComponent<State_Player>();
        #endregion
    }
}