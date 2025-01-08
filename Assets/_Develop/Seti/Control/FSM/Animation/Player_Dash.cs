using System;
using UnityEngine;

namespace Seti
{
    public class Player_Dash : Player_Base_AniState
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            context.Animator.SetBool(isMove, true);
            context.Animator.SetBool(isDash, true);
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            context.Animator.SetBool(isMove, false);
            context.Animator.SetBool(isDash, false);
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions() => null; // 기본적으로 전환 조건 없음

        // 상태 실행 중
        public override void Update()
        {

        }
        #endregion

        // 메서드
        #region Methods
        #endregion
    }
}