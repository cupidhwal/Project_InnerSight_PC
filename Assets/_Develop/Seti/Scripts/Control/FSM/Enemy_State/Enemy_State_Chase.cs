using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State_Chase : Enemy_State
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() => base.OnInitialized();

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            context.Actor.Condition.IsMove = true;
            context.Actor.Condition.IsChase = true;
            enemy.SwitchState(Enemy.State.Chase);
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();
            enemy.Condition.IsChase = false;
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (damagable.CurrentHitPoints <= 0)
                return typeof(Enemy_State_Dead);

            if (!condition.InAction)
                return typeof(Enemy_State_Stagger);

            if (enemy.TooFarFromHome || (!enemy.Detected && enemy.GoBackHome))
                return typeof(Enemy_State_BackOff);

            if (enemy.CanAttack)
                return typeof(Enemy_State_Attack_Normal);

            if (!enemy.Detected || !enemy.Player)
                return typeof(Enemy_State_Idle);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Move 행동 AI Input
            if (context.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                if (moveBehaviour is Move move)
                {
                    Input_Chase();
                    move.FSM_MoveInput(moveInput, true);
                }
        }
        #endregion

        // 메서드
        #region Methods
        private void Input_Chase()
        {
            if (!enemy.Player) return;

            Vector2 enemyPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
            Vector2 playerPos = Camera.main.WorldToScreenPoint(enemy.Player.transform.position);
            moveInput = playerPos - enemyPos;
        }
        #endregion
    }
}