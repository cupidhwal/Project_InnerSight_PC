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
        public override void OnEnter() { }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() { }

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
                    
                }
        }
        #endregion

        // 메서드
        #region Methods
        private void Input_Attack(float deltaTime)
        {
            // 카운트다운 진행
            steeringInterval -= deltaTime;
            if (steeringInterval <= 0)
            {
                // 카운트다운 완료 시 행동
                

                // 다음 카운트다운 시간 초기화
                steeringInterval = UnityEngine.Random.Range(0f, 2f);
            }
        }
        #endregion
    }
}