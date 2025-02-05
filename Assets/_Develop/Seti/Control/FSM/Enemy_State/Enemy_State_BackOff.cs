using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State_BackOff : Enemy_State
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() => base.OnInitialized();

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            enemy.SwitchState(Enemy.State.BackHome);

            if (damagable)
                damagable.IsInvulnerable = true;
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();

            if (damagable)
                damagable.IsInvulnerable = false;
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (damagable.CurrentHitPoints <= 0)
                return typeof(Enemy_State_Dead);

            if (enemy.ImHome)
                return typeof(Enemy_State_Idle);

            if (enemy.Detected && context.StateMachine.ElapsedTime > 2f)
                return typeof(Enemy_State_Chase);

            else return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Move 행동 AI Input
            if (context.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                if (moveBehaviour is Move move)
                {
                    Input_BackHome();
                    move.FSM_MoveInput(moveInput, true);
                }
        }
        #endregion

        // 메서드
        #region Methods
        private void Input_BackHome()
        {
            Vector2 enemyPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
            Vector2 homePos = Camera.main.WorldToScreenPoint(enemy.HomePosition);
            moveInput = homePos - enemyPos;
        }
        #endregion
    }
}