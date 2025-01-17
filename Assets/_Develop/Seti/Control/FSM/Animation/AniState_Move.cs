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
            //context.Animator.SetBool(isMove, true);
            context.Animator.SetBool(Hash_InputDetected, true);
            base.OnEnter();

            context.aniState = AniState.Move;
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            //context.Animator.SetBool(isMove, false);
            context.Animator.SetBool(Hash_InputDetected, false);
            base.OnExit();
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (!context.IsMove)
                return typeof(AniState_Idle);

            else if (context.IsDash)
                return typeof(AniState_Dash);
            
            else if (context.IsAttack)
                return typeof(AniState_Attack);
            
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