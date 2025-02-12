using System;

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
            context.Actor.Condition.IsMove = false;
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

            if (!enemy.Detected && enemy.CanMagic)
                return typeof(Enemy_State_Attack_Magic);

            if (context.StateMachine.ElapsedTime > elapsedDuration)
                return typeof(Enemy_State_Patrol);

            return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Move 행동 AI Input
            if (context.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
                if (moveBehaviour is Move move)
                {
                    move.FSM_MoveInput(moveInput, false);
                }
        }
        #endregion
    }
}