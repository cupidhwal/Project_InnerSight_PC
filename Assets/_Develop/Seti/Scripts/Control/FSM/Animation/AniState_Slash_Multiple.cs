using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Slash_Multiple : AniState_Base
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            context.Animator.SetTrigger(Hash_Slash_Multiple);
            context.currentState = AniState.Attack;
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

            if (!context.Actor.Condition.IsSlash_Multiple && !context.Actor.Condition.IsMove)
                return typeof(AniState_Idle);

            if (!context.Actor.Condition.IsSlash_Multiple && context.Actor.Condition.IsMove)
                return typeof(AniState_Move);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            //context.Animator.SetTrigger(Hash_Slash_Multiple);
        }
        #endregion
    }
}