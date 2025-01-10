using System;

namespace Seti
{
    public class Enemy_State_Idle : Enemy_State
    {
        // 추상
        #region Abstract
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized()
        {
            base.OnInitialized();
            elapsedMin = 3f;
            elapsedMax = 7f;
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

            else if (context.StateMachine.ElapsedTime > elapsedTime)
                return typeof(Enemy_State_Patrol);

            else return null;
        }

        // 상태 실행 중
        public override void Update(float deltaTime)
        {

        }
        #endregion
    }
}