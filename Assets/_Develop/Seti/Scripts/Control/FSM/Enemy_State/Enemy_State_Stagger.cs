using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State_Stagger : Enemy_State
    {
        // 오버라이드
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized()
        {
            base.OnInitialized();
        }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();

            elapsedDuration = context.Actor.Stagger;
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (damagable.CurrentHitPoints <= 0)
                return typeof(Enemy_State_Dead);

            if (enemy.Detected && context.StateMachine.ElapsedTime > elapsedDuration)
                return typeof(Enemy_State_Chase);

            if (!enemy.Detected && enemy.GoBackHome && context.StateMachine.ElapsedTime > elapsedDuration)
                return typeof(Enemy_State_BackOff);

            if (!enemy.Detected && context.StateMachine.ElapsedTime > elapsedDuration)
                return typeof(Enemy_State_Idle);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Move 행동 AI Input
            move?.FSM_MoveInput(Vector2.zero, false);
        }
    }
}