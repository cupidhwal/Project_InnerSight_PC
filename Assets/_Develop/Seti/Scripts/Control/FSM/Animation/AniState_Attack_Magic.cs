using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Attack_Magic : AniState_Base
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            context.Animator.SetTrigger(Hash_MagicAttack);
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

            if (!context.Actor.Condition.IsMagic && !context.Actor.Condition.IsMove)
                return typeof(AniState_Idle);

            if (!context.Actor.Condition.IsMagic && context.Actor.Condition.IsMove)
                return typeof(AniState_Move);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            MagicState();

            Debug.Log($"IsAttack: {context.Actor.Condition.IsAttack}");
            Debug.Log($"IsMagic: {context.Actor.Condition.IsMagic}");
        }
        #endregion

        //공격 처리
        void MagicState()
        {
            context.Animator.ResetTrigger(Hash_MagicAttack);

            context.Animator.SetFloat(Hash_StateTime,
                                      Mathf.Repeat(context.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            if (context.Actor.Condition.IsMagic)
                context.Animator.SetTrigger(Hash_MagicAttack);
        }
    }
}