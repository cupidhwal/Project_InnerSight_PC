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
            enemy.SwitchState(Enemy.State.Dead);


        }

        // 상태 전환 시 State Exit에 1회 실행
        public override void OnExit() => base.OnExit();

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