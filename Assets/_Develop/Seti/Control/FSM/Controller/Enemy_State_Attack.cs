using System;

namespace Seti
{
    public class Enemy_State_Attack : Enemy_State
    {
        // 추상
        #region Abstract
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() => base.OnInitialized();

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            enemy.SwitchState(Enemy.State.Attack);
            steeringInterval = enemy.AttackInterval;
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit()
        {
            base.OnExit();

            // Attack 행동 종료
            if (context.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                if (attackBehaviour is Attack attack)
                    attack?.FSM_AttackInput(false);
        }

        // 상태 전환 조건 메서드
        public override Type CheckTransitions()
        {
            if (!enemy.Detected)
                return typeof(Enemy_State_Idle);

            else if (!enemy.CanAttack)
                return typeof(Enemy_State_Chase);

            else return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {
            // Attack 행동 AI Input
            if (context.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                if (attackBehaviour is Attack attack)
                {
                    if (Input_Attack(deltaTime))
                        attack?.FSM_AttackInput(true);
                }
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
                steeringInterval = enemy.AttackInterval;

                // 카운트다운 완료 시 행동
                return true;
            }
            return false;
        }
        #endregion
    }
}