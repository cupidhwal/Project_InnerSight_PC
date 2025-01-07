using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// Move Function
    /// </summary>
    [System.Serializable]
    public class Move : IBehaviour, IHasStrategy
    {
        private enum MoveStrategies
        {
            Normal,
            //Run
        }

        // 필드
        #region Variables
        private float speed_Walk = 4f;

        private Actor actor;
        [SerializeReference]
        private List<Strategy> strategies;
        private IMoveStrategy currentStrategy;
        private Vector2 moveInput; // 입력 값

        // 상태 관리
        private MoveStrategies moveStrategies;
        #endregion

        // 인터페이스
        #region Interface
        // 업그레이드
        public void Upgrade(float increment)
        {
            float speed_Walk_Default = 4f;
            speed_Walk += increment * speed_Walk_Default / 100;

            /*float speed_Run_Default = 3.5f;
            speed_Run += increment * speed_Run_Default / 100;*/

            Initialize(actor);
        }

        // 초기화
        public void Initialize(Actor actor)
        {
            this.actor = actor;
            foreach (var mapping in strategies)
            {
                IMoveStrategy moveStrategy = mapping.strategy as IMoveStrategy;
                switch (moveStrategy)
                {
                    case Move_Normal:
                        moveStrategy.Initialize(actor, speed_Walk);
                        break;

                    /*case Move_Run:
                        moveStrategy.Initialize(actor, speed_Run);
                        break;*/
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
                //Debug.LogWarning("Move 전략이 없어 초기 전략을 설정하지 못했습니다.");
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

                /*case MoveStrategies.Run:
                    ChangeStrategy(typeof(Move_Run));
                    break;*/
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        public void FixedUpdate()
        {
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

        public void OnRunStarted(InputAction.CallbackContext _)
        {
            // 체공 중일 경우 착지까지 전략 변경 불가
            //moveStrategies = MoveStrategies.Run;
            State_Common state = actor.ActorState as State_Common;
            if (!state.IsGrounded) return;

            //ChangeStrategy(typeof(Move_Run));
        }

        public void OnRunCanceled(InputAction.CallbackContext _)
        {
            // 체공 중일 경우 착지까지 전략 변경 불가
            moveStrategies = MoveStrategies.Normal;
            State_Common state = actor.ActorState as State_Common;
            if (!state.IsGrounded) return;

            ChangeStrategy(typeof(Move_Normal));
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
    }
}