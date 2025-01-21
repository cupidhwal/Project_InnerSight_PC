using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Attack : AniState_Base
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
            context.currentState = AniState.Attack;
            context.Animator.SetTrigger(Hash_MeleeAttack);
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();
            Controller_Base controller = context.Actor.GetComponent<Controller_Base>();
            if (controller.BehaviourMap.TryGetValue(typeof(Attack), out var behaviour))
                if (behaviour is Attack attack)
                    attack?.OnAttack(false);
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (!context.IsAttack && !context.IsMove)
                return typeof(AniState_Idle);

            else if (!context.IsAttack && context.IsMove)
                return typeof(AniState_Move);
            
            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            //context.Animator.SetFloat(Hash_StateTime, context.AniMachine.ElapsedTime);
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