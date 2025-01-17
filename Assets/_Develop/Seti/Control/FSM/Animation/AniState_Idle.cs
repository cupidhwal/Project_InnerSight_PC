using System;
using UnityEngine;
using UnityEngine.Windows;

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
            if (context.IsMove)
            {
                return typeof(AniState_Move);
            }

            else if (context.IsDash)
            {
                return typeof(AniState_Dash);
            }
            
            else if (context.IsAttack)
            {
                context.Animator.SetTrigger(Hash_MeleeAttack);
                return typeof(AniState_Attack);
            }
            
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
            bool inputDetected = context.IsMove || /*m_Input.Jump ||*/ context.IsAttack;

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