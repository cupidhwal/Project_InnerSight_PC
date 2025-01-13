using UnityEngine;

namespace Seti
{
    public abstract class AniState_Base : MonoState<Controller_Animator>
    {
        // 필드
        #region Variables
        // Animator parameter
        protected int WhichAttack = Animator.StringToHash("WhichAttack");
        protected int isAttack = Animator.StringToHash("IsAttack");
        protected int isDeath = Animator.StringToHash("IsDeath");
        protected int isMove = Animator.StringToHash("IsMove");
        protected int isDash = Animator.StringToHash("IsDash");



        // Component
        protected Transform actorTransform;
        #endregion

        // 오버라이드
        #region Override
        public override void OnEnter() => context.Initialize();
        public override void OnExit() => context.Initialize();
        #endregion
    }
}