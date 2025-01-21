using System;

namespace Seti
{
    public class AniState_Stagger : AniState_Base
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            context.currentState = AniState.Stagger;
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();
            context.Animator.SetBool(Hash_InputDetected, true);
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (!context.IsStagger && !context.IsMove)
                return typeof(AniState_Idle);

            else if (!context.IsStagger && context.IsMove)
                return typeof(AniState_Move);

            else if (!context.IsStagger && context.IsDash)
                return typeof(AniState_Dash);

            else if (!context.IsStagger && context.IsAttack)
                return typeof(AniState_Attack);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
        #endregion
    }
}