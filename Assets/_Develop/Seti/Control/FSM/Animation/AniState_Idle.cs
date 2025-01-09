using System;

namespace Seti
{
    public class AniState_Idle : AniState_Base
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter() => base.OnEnter();

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() => base.OnExit();

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (context.IsMove)
                return typeof(AniState_Move);

            else if (context.IsDash)
                return typeof(AniState_Dash);
            
            else if (context.IsAttack)
                return typeof(AniState_Attack);
            
            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {

        }
        #endregion
    }
}