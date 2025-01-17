using UnityEngine;

namespace Seti
{
    public abstract class AniState_Base : MonoState<Controller_Animator>
    {
        // 필드
        #region Variables
        // Animator parameter
        protected int isDeath = Animator.StringToHash("IsDeath");
        protected int isDash = Animator.StringToHash("IsDash");

        // float
        protected int Hash_VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        protected int Hash_AirborneVerticalSpeed = Animator.StringToHash("AirborneVerticalSpeed");
        protected int Hash_ForwardSpeed = Animator.StringToHash("ForwardSpeed");
        protected int Hash_AngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
        protected int Hash_HurtFromX = Animator.StringToHash("HurtFromX");
        protected int Hash_HurtFromY = Animator.StringToHash("HurtFromY");
        protected int Hash_StateTime = Animator.StringToHash("StateTime");
        protected int Hash_FootFall = Animator.StringToHash("FootFall");

        // int
        protected int Hash_RandomIdle = Animator.StringToHash("RandomIdle");

        // bool
        protected int Hash_Grounded = Animator.StringToHash("Grounded");
        protected int Hash_InputDetected = Animator.StringToHash("InputDetected");

        // trigger
        protected int Hash_TimeoutToIdle = Animator.StringToHash("TimeoutToIdle");
        protected int Hash_MeleeAttack = Animator.StringToHash("MeleeAttack");
        protected int Hash_Hurt = Animator.StringToHash("Hurt");
        protected int Hash_Death = Animator.StringToHash("Death");
        protected int Hash_Respawn = Animator.StringToHash("Respawn");
        #endregion

        // 오버라이드
        #region Override
        public override void OnEnter() => context.Initialize();
        public override void OnExit() => context.Initialize();
        #endregion
    }
}