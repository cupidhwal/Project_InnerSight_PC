using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Dash : AniState_Base
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();

            context.Animator.SetTrigger(OnDash);
            context.currentState = AniState.Dash;
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (context.Actor.Condition.IsDead)
                return typeof(AniState_Die);

            if (!Condition_Player.IsDash && !context.Actor.Condition.IsMove)
                return typeof(AniState_Idle);
            
            if (!Condition_Player.IsDash && context.Actor.Condition.IsMove)
                return typeof(AniState_Move);

            if (!Condition_Player.IsDash && context.Actor.Condition.IsStagger)
                return typeof(AniState_Stagger);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime) => base.Update(deltaTime);
        #endregion

        // 메서드
        #region Methods
        #endregion
    }
}