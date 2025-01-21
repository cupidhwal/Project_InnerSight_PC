using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Die : AniState_Base
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            context.Animator.SetTrigger(Hash_Death);
            context.currentState = AniState.Dead;
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() { }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions() => null;

        // 상태 실행 중
        public override void Update(float deltaTime) { }
        #endregion
    }
}