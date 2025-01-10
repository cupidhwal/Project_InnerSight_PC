using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State : MonoState<Controller_FSM>
    {
        // 필드
        #region Variables
        protected Enemy enemy;

        protected float elapsedTime;    // 시간 경과
        protected float elapsedMin = 5f;
        protected float elapsedMax = 8f;
        protected float steeringInterval;

        // Patrol, Chase
        protected Vector2 moveInput;
        #endregion

        // 추상
        #region Abstract
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized()
        {
            enemy = context.Actor as Enemy;
            elapsedTime = 0f;
        }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter() => elapsedTime = UnityEngine.Random.Range(elapsedMin, elapsedMax);

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() { }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions() => null; // 기본적으로 전환 조건 없음

        // 상태 실행 중
        public override void Update(float deltaTime)
        {

        }
        #endregion
    }
}