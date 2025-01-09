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

        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            context.Animator.SetInteger(WhichAttack, AttackCombo());
            context.Animator.SetBool(isAttack, true);
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            context.Animator.SetBool(isAttack, false);
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
        private int AttackCombo() => comboIndex++ % comboCount;
        #endregion
    }
}