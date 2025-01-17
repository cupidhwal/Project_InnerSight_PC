using System;
using UnityEngine;

namespace Seti
{
    public abstract class Enemy_State : MonoState<Controller_FSM>
    {
        // 필드
        #region Variables
        protected Enemy enemy;
        protected Condition_Enemy condition;
        protected Damagable damagable;
        protected float elapsedTime = 5f;       // 상태 전이 시간 경과
        protected float elapsedCriteria = 10f;  // 상태 전이 시간 경과 기준
        protected float steeringInterval;       // 상태 조작 주기

        // Patrol, Chase
        protected Vector2 moveInput;
        #endregion

        // 추상
        #region Abstract
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized()
        {
            enemy = context.Actor as Enemy;
            condition = enemy.GetComponent<Condition_Enemy>();

            if (context.TryGetComponent<Damagable>(out var damagable))
            {
                this.damagable = damagable;
            }
        }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter() => elapsedTime = UnityEngine.Random.Range(elapsedCriteria * 0.7f, elapsedCriteria * 1.3f);

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() { }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions() => null;
        #endregion
    }
}