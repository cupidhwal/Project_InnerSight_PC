using Noah;
using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State_Idle : Enemy_State
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized()
        {
            base.OnInitialized();
            elapsedCriteria = 5f;
        }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            enemy.SwitchState(Enemy.State.Idle);
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() => base.OnExit();

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (damagable.CurrentHitPoints <= 0)
                return typeof(Enemy_State_Dead);

            if (!condition.InAction)
                return typeof(Enemy_State_Stagger);

            if (enemy.Detected)
                return typeof(Enemy_State_Chase);

            else if (context.StateMachine.ElapsedTime > elapsedTime)
                return typeof(Enemy_State_Patrol);

            else return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Move 행동 AI Input
            Debug.Log(context);
            Debug.Log(context.BehaviourMap);
            if (context.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                if (moveBehaviour is Move move)
                {
                    Debug.Log(moveBehaviour);
                    Debug.Log(move);
                    move.FSM_MoveInput(moveInput, false);
                }
        }
        #endregion
    }
}