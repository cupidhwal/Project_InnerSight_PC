using System;
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
        private enum MoveStrategies
        {
            Normal,
            Dash,
            Walk,
            Run
        }

        // 필드
        #region Variables
        // Player - 기본 이동
        private float speed_Move = 4f;
        private const float speed_Move_Default = 4f;

        // Player - 대시
        public float Speed_Dash => speed_Move * 10f;
        private const float delay_Dash = 0.15f;
        private const float coolDown_Dash = 4f;
        private bool isDashed = false;

        // 몬스터
        private const float speed_Walk = 2f;
        private const float speed_Run = 3.5f;

        private Actor actor;
        [SerializeReference]
        private List<Strategy> strategies;
        private IMoveStrategy currentStrategy;
        private Vector2 moveInput; // 입력 값

        // 상태 관리
        private MoveStrategies moveStrategies;
        private State_Common state;
        #endregion

        // 인터페이스
        #region Interface
        // 업그레이드
        public void Upgrade(float increment)
        {
            speed_Move += increment * speed_Move_Default / 100;
            Initialize(actor);
        }

        // 초기화
        public void Initialize(Actor actor)
        {
            this.actor = actor;
            state = actor.ActorState as State_Common;
            foreach (var mapping in strategies)
            {
                IMoveStrategy moveStrategy = mapping.strategy as IMoveStrategy;
                switch (moveStrategy)
                {
                    case Move_Normal:
                        moveStrategy.Initialize(actor, speed_Move);
                        break;

                    case Move_Dash:
                        moveStrategy.Initialize(actor, Speed_Dash);
                        break;

                    case Move_Walk:
                        moveStrategy.Initialize(actor, speed_Walk);
                        break;

                    case Move_Run:
                        moveStrategy.Initialize(actor, speed_Run);
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

        private void SwitchStrategy()
        {
            switch (moveStrategies)
            {
                case MoveStrategies.Normal:
                    ChangeStrategy(typeof(Move_Normal));
                    break;

                case MoveStrategies.Dash:
                    ChangeStrategy(typeof(Move_Dash));
                    break;

                case MoveStrategies.Walk:
                    ChangeStrategy(typeof(Move_Walk));
                    break;

                case MoveStrategies.Run:
                    ChangeStrategy(typeof(Move_Run));
                    break;
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        public void FixedUpdate()
        {
            // 공격 중일 땐 이동 불가
            State_Common state = actor.ActorState as State_Common;
            if (state.IsAttack) return;

            currentStrategy?.Move(moveInput);
        }
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        public void OnMovePerformed(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnMoveCanceled(InputAction.CallbackContext _)
        {
            moveInput = Vector2.zero;
        }

        public void OnDashStarted(InputAction.CallbackContext _)
        {
            // 체공 중일 경우 착지까지 전략 변경 불가
            if (!state.IsGrounded || isDashed) return;

            Execute_Dash();
        }

        public void OnRunStarted(InputAction.CallbackContext _)
        {
            // 체공 중일 경우 착지까지 전략 변경 불가
            if (!state.IsGrounded) return;

            moveStrategies = MoveStrategies.Run;
            SwitchStrategy();
        }

        public void OnRunCanceled(InputAction.CallbackContext _)
        {
            // 체공 중일 경우 착지까지 전략 변경 불가
            if (!state.IsGrounded) return;

            moveStrategies = MoveStrategies.Normal;
            SwitchStrategy();
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        public void OnCollisionEnter(Collision collision)
        {
            currentStrategy?.GetOverCurb(collision);
            SwitchStrategy();
        }
        #endregion

        // 유틸리티
        #region Utilities
        private async void Execute_Dash()
        {
            isDashed = true;

            moveStrategies = MoveStrategies.Dash;
            SwitchStrategy();

            await Task.Delay((int)(delay_Dash * 1000));
            if (currentStrategy is Move_Dash dash)
                dash.MoveExit();

            moveStrategies = MoveStrategies.Normal;
            SwitchStrategy();

            await Task.Delay((int)((coolDown_Dash - delay_Dash) * 1000));
            isDashed = false;
        }
        #endregion
    }
}