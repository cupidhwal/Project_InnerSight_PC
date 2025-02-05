namespace Seti
{
    public class Enemy_State_Dead : Enemy_State
    {
        // 오버라이드
        #region Override
        // 초기화 메서드 - 생성 후 1회 실행
        public override void OnInitialized() => base.OnInitialized();

        // 상태 전환 시 State Enter에 1회 실행
        public override void OnEnter()
        {
            base.OnEnter();
            elapsedDuration = 100;
            context.Actor.Condition.IsDead = true;
            context.Actor.Controller_Animator.IsDead = true;
            enemy.SwitchState(Enemy.State.Dead);
        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() => base.OnExit();

        // 상태 실행 중
        public override void Update(float deltaTime) { }
        #endregion
    }
}