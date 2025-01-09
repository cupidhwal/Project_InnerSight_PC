using System;
using UnityEngine;

namespace Seti
{
    public class State_Chase : MonoState<Controller_FSM>
    {
        // 추상
        #region Abstract
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter() { }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() { }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (!context.Detected)
                return typeof(State_Idle);

            else if (context.CanAttack)
                return typeof(State_Attack);

            else return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {

        }
        #endregion

        // 메서드
        #region Methods
        #endregion
    }
}