using System;
using System.Collections;
using System.Collections.Generic;
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
        // 필드
        #region Variables
        // 전략 관리
        private Actor actor;
        private Player player;
        private Condition_Player condition_Player;
        [SerializeReference]
        private List<Strategy> strategies;
        private IMoveStrategy currentStrategy;

        // 제어 관리
        private Vector2 moveInput;  // 입력 값
        public Vector2 MoveInput => moveInput;
        private State<Controller_FSM> currentState;
        #endregion

        // 인터페이스
        #region Interface
        // 초기화
        public void Initialize(Actor actor)
        {
            this.actor = actor;
            if (actor is Player)
            {
                player = actor as Player;
                condition_Player = actor.Condition as Condition_Player;
            }
            foreach (var mapping in strategies)
            {
                IMoveStrategy moveStrategy = mapping.strategy as IMoveStrategy;
                switch (moveStrategy)
                {
                    case Move_Normal:
                        moveStrategy.Initialize(actor);
                        break;

                    case Move_Walk:
                        moveStrategy.Initialize(actor);
                        break;

                    case Move_Run:
                        moveStrategy.Initialize(actor);
                        break;

                    case Move_Nav:
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

        public void SwitchStrategy(State<Controller_FSM> state)
        {
            // FSM 상태에 따라 동작 제어
            currentState = state;
            switch (currentState)
            {
                case Enemy_State_Patrol:
                    ChangeStrategy(typeof(Move_Walk));
                    break;

                case Enemy_State_Chase:
                    ChangeStrategy(typeof(Move_Run));
                    break;

                case Enemy_State_BackOff:
                    ChangeStrategy(typeof(Move_Nav));
                    break;
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        public void Update()
        {
            if (!actor.Condition.InAction || condition_Player.IsDash) return;
            currentStrategy?.Move(moveInput);
        }
        #endregion

        // 컨트롤러
        #region Controllers
        #region Controller_Input
        public void OnMovePerformed(InputAction.CallbackContext context) => OnMove(context.ReadValue<Vector2>(), true);
        public void OnMoveCanceled(InputAction.CallbackContext _) => OnMove(Vector2.zero, false);
        #endregion

        #region Controller_FSM
        public void FSM_MoveInput(Vector2 moveInput, bool isMove) => OnMove(moveInput, isMove);
        #endregion
        #endregion

        // 이벤트 메서드
        #region Event Methods
        public void OnCollisionEnter(Collision collision)
        {
            currentStrategy?.GetOverCurb(collision);
        }
        #endregion

        // 메서드
        #region Methods
        public void OnMove(Vector2 moveInput, bool isMove)
        {
            this.moveInput = moveInput;
            actor.Condition.IsMove = isMove;
        }
        #endregion
    }
}