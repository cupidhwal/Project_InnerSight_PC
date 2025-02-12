using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Idle : AniState_Base
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            context.currentState = AniState.Idle;
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
            if (context.Actor.Condition.IsDead)
                return typeof(AniState_Die);

            if (context.Actor.Condition.IsMove)
                return typeof(AniState_Move);

            if (context.Actor.Condition is Condition_Player condition_Player && condition_Player.IsDash)
                return typeof(AniState_Dash);
            
            if (context.Actor.Condition.IsMagic)
                return typeof(AniState_Attack_Magic);

            if (context.Actor.Condition.IsAttack)
                return typeof(AniState_Attack_Melee);

            if (context.Actor.Condition.IsStagger)
                return typeof(AniState_Stagger);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            TimoutToIdle();
        }
        #endregion






        //이동상태의 대기에서 대기시간(5초)이 지나면 대기 상태로 보낸다
        float idleTimer;
        const float idleCriteria = 5f;
        void TimoutToIdle()
        {
            //입력값 체크(이동, 공격)
            bool inputDetected = context.Actor.Condition.IsMove || context.Actor.Condition.IsMagic || context.Actor.Condition.IsAttack;

            //타이머 카운트
            if (context.Actor.Condition.IsGrounded && !inputDetected)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleCriteria)
                {
                    context.Animator.SetTrigger(Hash_TimeoutToIdle);

                    //초기화
                    idleTimer = 0;
                }
            }
            else
            {
                //초기화
                idleTimer = 0;
                context.Animator.ResetTrigger(Hash_TimeoutToIdle);
            }

            //애니 입력값 설정
            context.Animator.SetBool(Hash_InputDetected, inputDetected);
        }
    }
}