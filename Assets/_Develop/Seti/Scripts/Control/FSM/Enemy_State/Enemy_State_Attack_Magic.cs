using System;
using UnityEngine;

namespace Seti
{
    public class Enemy_State_Attack_Magic : Enemy_State
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized()
        {
            base.OnInitialized();

            steeringInterval = enemy.MagicInterval;
        }

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();

            attack?.FSM_MagicInput(true);
            enemy.SwitchState(Enemy.State.Attack);
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();

            // Attack 행동 종료
            attack?.FSM_MagicInput(false);
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (damagable.CurrentHitPoints <= 0)
                return typeof(Enemy_State_Dead);

            if (!condition.InAction)
                return typeof(Enemy_State_Stagger);

            if (enemy.Detected && enemy.Condition.CanMove)
                return typeof(Enemy_State_Chase);

            if ((!enemy.Condition.IsMagic && !enemy.CanMagic) || enemy.Player.Condition.IsDead)
                return typeof(Enemy_State_Idle);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Move 행동 AI Input
            move?.FSM_MoveInput(Vector2.zero, false);

            // Look 행동 AI Input
            look?.FSM_LookInput();

            // Attack 행동 AI Input
            if (Input_Attack(deltaTime))
                attack?.FSM_MagicInput(true);
            else attack?.FSM_MagicInput(false);
        }
        #endregion

        // 메서드
        #region Methods
        private bool Input_Attack(float deltaTime)
        {
            // 카운트다운 진행
            steeringInterval -= deltaTime;
            if (steeringInterval <= 0)
            {
                // 공격 주기가 변경된 경우 갱신
                steeringInterval = enemy.MagicInterval;

                // 카운트다운 완료 시 행동
                return true;
            }
            return false;
        }
        #endregion
    }
}