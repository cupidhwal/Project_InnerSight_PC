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
        protected float elapsedDuration = 5f;       // 상태 전이 시간 경과
        protected float elapsedCriteria = 10f;  // 상태 전이 시간 경과 기준
        protected float steeringInterval;       // 상태 조작 주기

        // Patrol, Chase
        protected Vector2 moveInput;

        protected Move move;
        protected Look look;
        protected Attack attack;
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

            // 다른 행동을 참조해야 한다면 이런 양식으로 작성
            if (context.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                move = moveBehaviour as Move;

            if (context.BehaviourMap.TryGetValue(typeof(Look), out var lookBehaviour))
                look = lookBehaviour as Look;

            if (context.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                attack = attackBehaviour as Attack;
        }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            elapsedDuration = UnityEngine.Random.Range(elapsedCriteria * 0.7f, elapsedCriteria * 1.3f);
            enemy.Agent.ResetPath();
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() { }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions() => null;
        #endregion
    }
}