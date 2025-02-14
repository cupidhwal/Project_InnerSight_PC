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

            enemy.Agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            enemy.Agent.SetDestination(enemy.HomePosition);

            context.Actor.Condition.IsMove = true;

            if (damagable)
            {
                damagable.IsInvulnerable = true;
                damagable.OnReceiveDamage += ReturnToChase;
            }
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();

            enemy.Agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

            if (damagable)
            {
                damagable.IsInvulnerable = false;
                damagable.OnReceiveDamage -= ReturnToChase;
            }
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (damagable.CurrentHitPoints <= 0)
                return typeof(Enemy_State_Dead);

            if (enemy.ImHome)
                return typeof(Enemy_State_Idle);

            if (enemy.Detected && context.StateMachine.ElapsedTime > 5f)
                return typeof(Enemy_State_Chase);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Move 행동 AI Input
            if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
            {
                enemy.Agent.ResetPath();
            }

            Input_BackHome();
            move?.FSM_MoveInput(moveInput, true);
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

        private void ReturnToChase()
        {
            enemy.Agent.SetDestination(enemy.transform.position);
            context.StateMachine.ChangeState<Enemy_State_Chase>();
        }
        #endregion
    }
}