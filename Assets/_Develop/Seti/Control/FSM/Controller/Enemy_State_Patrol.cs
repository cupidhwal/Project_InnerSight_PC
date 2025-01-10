using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State_Patrol : Enemy_State
    {
        // 추상
        #region Abstract
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized()
        {
            base.OnInitialized();
            elapsedMin = 5f;
            elapsedMax = 15f;
            steeringInterval = 0f;
        }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter() { }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() { }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (enemy.Detected)
                return typeof(Enemy_State_Chase);

            else if (!enemy.Detected && context.StateMachine.ElapsedTime > elapsedTime)
                return typeof(Enemy_State_Idle);

            else return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Move 행동 AI Input
            if (context.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                if (moveBehaviour is Move move)
                {
                    Input_Patrol(deltaTime);
                    move.FSM_MoveInput(moveInput);
                }
        }
        #endregion

        // 메서드
        #region Methods
        private void Input_Patrol(float deltaTime)
        {
            // 카운트다운 진행
            steeringInterval -= deltaTime;
            if (steeringInterval <= 0)
            {
                // 카운트다운 완료 시 행동
                moveInput = GenRandomVec2();

                // 다음 카운트다운 시간 초기화
                steeringInterval = UnityEngine.Random.Range(0f, 2f);
            }
        }
        private Vector2 GenRandomVec2()
        {
            float x = UnityEngine.Random.Range(-1f, 1f);
            float y = UnityEngine.Random.Range(-1f, 1f);
            return new Vector2(x, y);
        }
        #endregion
    }
}