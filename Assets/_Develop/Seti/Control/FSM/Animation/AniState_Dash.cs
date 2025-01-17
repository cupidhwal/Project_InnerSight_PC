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
            //context.Animator.SetBool(isMove, true);
            /*context.Animator.SetBool(Hash_InputDetected, true);
            context.Animator.SetBool(isDash, true);*/
            base.OnEnter();

            context.aniState = AniState.Dash;
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            //context.Animator.SetBool(isMove, false);
            /*context.Animator.SetBool(Hash_InputDetected, false);
            context.Animator.SetBool(isDash, false);*/
            base.OnExit();
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (!context.IsDash && context.IsMove)
                return typeof(AniState_Move);
            
            else if (!context.IsDash && !context.IsMove)
                return typeof(AniState_Idle);
            
            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            context.Initialize();
        }
        #endregion

        // 메서드
        #region Methods
        #endregion
    }
}