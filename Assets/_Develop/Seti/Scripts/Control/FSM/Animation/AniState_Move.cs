using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Move : AniState_Base
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();

            context.currentState = AniState.Move;
            context.Animator.SetBool(Hash_InputDetected, true);
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();

            context.Animator.SetBool(Hash_InputDetected, false);
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (context.Actor.Condition.IsDead)
                return typeof(AniState_Die);

            if (!context.Actor.Condition.IsMove)
                return typeof(AniState_Idle);

            if (Player && Condition_Player.IsDash)
                return typeof(AniState_Dash);
            
            if (context.Actor.Condition.IsMagic)
                return typeof(AniState_Attack_Magic);

            if (context.Actor.Condition.IsAttack)
                return typeof(AniState_Attack_Melee);

            if (context.Actor.Condition.IsSlash_Double)
                return typeof(AniState_Slash_Double);

            if (context.Actor.Condition.IsSlash_Multiple)
                return typeof(AniState_Slash_Multiple);

            if (context.Actor.Condition.IsStagger)
                return typeof(AniState_Stagger);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime) => base.Update(deltaTime);
        #endregion
    }
}