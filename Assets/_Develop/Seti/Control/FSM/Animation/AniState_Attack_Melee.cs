using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Attack_Melee : AniState_Base
    {
        // 필드
        #region Variables
        private int comboIndex;
        private int comboCount = 2;
        #endregion

        // 속성
        #region Properties
        private int AttackCombo => comboIndex++ % comboCount;
        #endregion

        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            context.Animator.SetTrigger(Hash_MeleeAttack);
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

            if (!context.Actor.Condition.IsAttack && !context.Actor.Condition.IsMove)
                return typeof(AniState_Idle);

            else if (!context.Actor.Condition.IsAttack && context.Actor.Condition.IsMove)
                return typeof(AniState_Move);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            AttackState();
        }
        #endregion





        //공격 처리
        void AttackState()
        {
            context.Animator.ResetTrigger(Hash_MeleeAttack);

            context.Animator.SetFloat(Hash_StateTime,
                                      Mathf.Repeat(context.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            if (context.Actor.Condition.IsAttack)
                context.Animator.SetTrigger(Hash_MeleeAttack);
        }
    }
}