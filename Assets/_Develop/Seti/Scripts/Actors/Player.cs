using System.Collections;
using UnityEngine;

namespace Seti
{
    // Player가 가져야 할 Component
    [RequireComponent(typeof(Enhance))]

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
        protected float moveSpeed = 4f;
        [SerializeField]
        protected float dashSpeed = 20f;
        [SerializeField]
        protected float dashDuration = 0.10f;
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

        protected override void Start()
        {
            base.Start();

            StartCoroutine(Upgrade(1));
        }

        private IEnumerator Upgrade(int count)
        {
            Enhance enhance = GetComponent<Enhance>();
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(1);
                enhance.EnhanceBehaviour<Health>();
                enhance.EnhanceBehaviour<Attack>();
                enhance.EnhanceBehaviour<Defend>();
                enhance.EnhanceBehaviour<Move>();
            }
            yield break;
        }
    }
}