using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// Move Behaviour
    /// </summary>
    [System.Serializable]
    public class Move : IBehaviour, IHasStrategy
    {
        private enum StrategyType
        {
            Normal,
            Dash,
            Walk,
            Run,
            NULL
        }

        // 필드
        #region Variables
        // Player
        private float speed_Move = 4f;
        private bool isDashed = false;

        // 전략 관리
        private Actor actor;
        private Player player;
        [SerializeReference]
        private List<Strategy> strategies;
        private IMoveStrategy currentStrategy;

        // 제어 관리
        private Vector2 moveInput;  // 입력 값
        public Vector2 MoveInput => moveInput;
        private StrategyType currentType;
        private State<Controller_FSM> currentState;
        #endregion

        // 인터페이스
        #region Interface
        // 업그레이드
        public void Upgrade(float increment)
        {
            if (actor is not Player) return;

            speed_Move = actor.Rate_Movement;
            speed_Move += increment * player.Rate_Movement_Default / 100;
            Initialize(actor);
        }

        // 초기화
        public void Initialize(Actor actor)
        {
            this.actor = actor;
            if (actor is Player)
                player = actor as Player;
            foreach (var mapping in strategies)
            {
                IMoveStrategy moveStrategy = mapping.strategy as IMoveStrategy;
                switch (moveStrategy)
                {
                    case Move_Normal:
                        moveStrategy.Initialize(actor);
                        break;

                    case Move_Dash:
                        moveStrategy.Initialize(actor);
                        break;

                    case Move_Walk:
                        moveStrategy.Initialize(actor);
                        break;

                    case Move_Run:
                        moveStrategy.Initialize(actor);
                        break;
                }
            }

            // 초기 전략 설정
            var defaultStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy is Move_Normal);
            if (defaultStrategy != null)
            {
                ChangeStrategy(typeof(Move_Normal));
            }
            else if (strategies.Count > 0)
            {
                ChangeStrategy(strategies[0].strategy.GetType());
            }
            else
            {
                Debug.LogWarning("Move 전략이 없어 초기 전략을 설정하지 못했습니다.");
                ChangeStrategy(null);
            }
        }

        public Type GetBehaviourType() => typeof(Move);
        public Type GetStrategyType() => typeof(IMoveStrategy);

        // 보유 전략 확인
        public bool HasStrategy<T>() where T : class, IStrategy => strategies.Any(strategy => strategy.strategy is T);

        // 행동 전략 설정
        public void SetStrategies(IEnumerable<Strategy> strategies)
        {
            this.strategies = strategies.ToList(); // 전달받은 전략 리스트 저장
        }

        // 행동 전략 변경
        public void ChangeStrategy(Type strategyType)
        {
            var moveStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy.GetType() == strategyType);
            if (moveStrategy != null)
            {
                currentStrategy = moveStrategy.strategy as IMoveStrategy;
            }
        }

        private void SwitchStrategy(StrategyType type)
        {
            currentType = type;
            switch (currentType)
            {
                case StrategyType.Normal:
                    ChangeStrategy(typeof(Move_Normal));
                    break;

                case StrategyType.Dash:
                    ChangeStrategy(typeof(Move_Dash));
                    break;

                case StrategyType.Walk:
                    ChangeStrategy(typeof(Move_Walk));
                    break;

                case StrategyType.Run:
                    ChangeStrategy(typeof(Move_Run));
                    break;

                default:
                    currentStrategy = null;
                    break;
            }
        }
        private void SwitchStrategy()
        {
            switch (currentType)
            {
                case StrategyType.Normal:
                    ChangeStrategy(typeof(Move_Normal));
                    break;

                case StrategyType.Dash:
                    ChangeStrategy(typeof(Move_Dash));
                    break;

                case StrategyType.Walk:
                    ChangeStrategy(typeof(Move_Walk));
                    break;

                case StrategyType.Run:
                    ChangeStrategy(typeof(Move_Run));
                    break;

                default:
                    currentStrategy = null;
                    break;
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        public void Update()
        {
            currentStrategy?.Move(moveInput);
        }
        #endregion

        // 컨트롤러
        #region Controllers
        #region Controller_Input
        public void OnMovePerformed(InputAction.CallbackContext context) => OnMove(context.ReadValue<Vector2>(), true);
        public void OnMoveCanceled(InputAction.CallbackContext _) => OnMove(Vector2.zero, false);
        public void OnDashStarted(InputAction.CallbackContext _) => OnDash();
        public void OnRunStarted(InputAction.CallbackContext _) => OnRun(StrategyType.Run);
        public void OnRunCanceled(InputAction.CallbackContext _) => OnRun(StrategyType.Walk);
        #endregion

        #region Controller_FSM
        public void FSM_MoveInput(Vector2 moveInput, bool isMove) => OnMove(moveInput, isMove);
        public void FSM_MoveSwitch(State<Controller_FSM> state)
        {
            // FSM 상태에 따라 동작 제어
            currentState = state;
            switch (currentState)
            {
                case Enemy_State_Patrol:
                    SwitchStrategy(StrategyType.Walk);
                    break;

                case Enemy_State_Chase:
                    SwitchStrategy(StrategyType.Run);
                    break;

                case Enemy_State_BackOff:
                    SwitchStrategy(StrategyType.Run);
                    break;

                default:
                    SwitchStrategy(StrategyType.NULL);
                    break;
            }
        }
        #endregion
        #endregion

        // 이벤트 메서드
        #region Event Methods
        public void OnCollisionEnter(Collision collision)
        {
            currentStrategy?.GetOverCurb(collision);
            SwitchStrategy();
        }
        #endregion

        // 메서드
        #region Methods
        public void OnMove(Vector2 moveInput, bool isMove)
        {
            this.moveInput = moveInput;
            actor.Condition.IsMove = isMove;
        }

        private void OnDash()
        {
            // 체공 중일 경우 착지까지 전략 변경 불가
            if (isDashed || !actor.Condition.IsGrounded || !actor.Condition.CanMove) return;
            actor.CoroutineExecutor(Dash_Cor());
        }

        private void OnRun(StrategyType type)
        {
            currentType = type;

            // 체공 중일 경우 착지까지 전략 변경 불가
            //if (!state.IsGrounded) return;

            SwitchStrategy();
        }
        #endregion

        // 유틸리티
        #region Utilities
        private IEnumerator Dash_Cor()
        {
            // 대시 중 충돌 무시
            Collider collider = actor.GetComponent<Collider>();
            collider.excludeLayers = LayerMask.GetMask("Actor");

            // Damagable 컴포넌트가 있다면 대시 중 무적
            if (actor.TryGetComponent<Damagable>(out var damagable))
                damagable.IsInvulnerable = true;

            // 대시 시작
            isDashed = true;

            actor.Condition.IsDash = true;
            SwitchStrategy(StrategyType.Dash);

            // 대시 끝
            yield return new WaitForSeconds(player.Dash_Duration);
            if (currentStrategy is Move_Dash dash)
                dash.MoveExit();

            actor.Condition.IsDash = false;
            SwitchStrategy(StrategyType.Normal);

            // Damagable 컴포넌트가 있다면 무적 해제
            yield return new WaitForSeconds(player.Dash_Duration);
            damagable.IsInvulnerable = false;

            yield return new WaitForSeconds(player.Dash_Cooldown - (2 * player.Dash_Duration));
            isDashed = false;
            // 대시 사용 가능

            // 충돌 확인 재개
            collider.excludeLayers = LayerMask.GetMask("Nothing");

            yield break;
        }
        #endregion
    }
}