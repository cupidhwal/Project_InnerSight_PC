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
            Controller_Base controller = context.Actor.GetComponent<Controller_Base>();
            if (controller.BehaviourMap.TryGetValue(typeof(Attack), out var behaviour))
                if (behaviour is Attack attack)
                    attack?.OnMagic(false);
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (context.IsDead)
                return typeof(AniState_Die);

            if (!context.IsAttack && !context.IsMove)
                return typeof(AniState_Idle);

            else if (!context.IsAttack && context.IsMove)
                return typeof(AniState_Move);

            else if (context.IsAttack)
                return typeof(AniState_Attack_Melee);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            //AttackState();
        }
        #endregion





        //공격 처리
        void AttackState()
        {
            context.Animator.ResetTrigger(Hash_MagicAttack);

            context.Animator.SetFloat(Hash_StateTime,
                                      Mathf.Repeat(context.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            if (context.IsMagic)
                context.Animator.SetTrigger(Hash_MagicAttack);
        }
    }
}