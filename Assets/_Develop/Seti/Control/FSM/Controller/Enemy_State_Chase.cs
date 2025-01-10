using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State_Chase : Enemy_State
    {
        // 추상
        #region Abstract
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() => base.OnInitialized();

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter() { }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() { }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (!enemy.Detected)
                return typeof(Enemy_State_Idle);

            else if (enemy.CanAttack)
                return typeof(Enemy_State_Attack);

            else return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Move 행동 AI Input
            if (context.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                if (moveBehaviour is Move move)
                {
                    Input_Chase();
                    move.FSM_MoveInput(moveInput);
                }
        }
        #endregion

        // 메서드
        #region Methods
        private void Input_Chase()
        {
            Vector2 enemyPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
            Vector2 playerPos = Camera.main.WorldToScreenPoint(enemy.Player.transform.position);
            moveInput = playerPos - enemyPos;
        }
        #endregion
    }
}