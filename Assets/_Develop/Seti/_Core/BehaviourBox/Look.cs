using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// Look Function
    /// </summary>
    [System.Serializable]
    public class Look : IBehaviour, IHasStrategy
    {
        // 필드
        #region Variables
        // 전략 관리
        private Actor actor;
        [SerializeReference]
        private List<Strategy> strategies;
        private ILookStrategy currentStrategy;
        private Vector2 lookInput;

        // 제어 관리
        private State<Controller_FSM> currentState;
        #endregion

        // 인터페이스
        #region Interface
        // 초기화
        public void Initialize(Actor actor)
        {
            this.actor = actor;
            foreach (var mapping in strategies)
            {
                ILookStrategy lookStrategy = mapping.strategy as ILookStrategy;
                switch (lookStrategy)
                {
                    case Look_Normal:
                        lookStrategy.Initialize(actor, 0.1f);
                        break;

                    case Look_Watch:
                        lookStrategy.Initialize(actor);
                        break;
                }
            }

            // 초기 전략 설정
            var defaultStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy is Look_Normal);
            if (defaultStrategy != null)
            {
                ChangeStrategy(typeof(Look_Normal));
            }
            else if (strategies.Count > 0)
            {
                ChangeStrategy(strategies[0].strategy.GetType());
            }
            else
            {
                //Debug.LogWarning("Look 전략이 없어 초기 전략을 설정하지 못했습니다.");
                ChangeStrategy(null);
            }
        }

        public Type GetBehaviourType() => typeof(Look);
        public Type GetStrategyType() => typeof(ILookStrategy);

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
            var lookStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy.GetType() == strategyType);
            if (lookStrategy != null)
            {
                currentStrategy = lookStrategy.strategy as ILookStrategy;
            }
        }

        public void SwitchStrategy(State<Controller_FSM> state)
        {
            // FSM 상태에 따라 동작 제어
            currentState = state;
            switch (currentState)
            {
                case Enemy_State_Attack_Normal:
                    ChangeStrategy(typeof(Look_Watch));
                    break;

                case Enemy_State_Attack_Magic:
                    ChangeStrategy(typeof(Look_Watch));
                    break;

                default:
                    ChangeStrategy(null);
                    break;
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        public void Update()
        {
            if (!actor.Condition.InAction) return;
            currentStrategy?.Look();
        }
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        #region Controller_Input
        public void OnLookPerformed(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
            currentStrategy?.Look(lookInput);
        }

        public void OnLookCanceled(InputAction.CallbackContext _)
        {
            lookInput = Vector2.zero;
            currentStrategy?.Look(lookInput);
        }

        public void OnKeepGoingStarted(InputAction.CallbackContext _)
        {
            //ChangeStrategy(typeof(Look_KeepGoing));
        }

        public void OnKeepGoingCanceled(InputAction.CallbackContext _)
        {
            ChangeStrategy(typeof(Look_Normal));
        }
        #endregion

        #region Controller_FSM
        public void FSM_LookInput() => currentStrategy?.Look();
        #endregion
        #endregion
    }
}

#region Dummy
/*public void SetStrategy(Blueprint_Actor blueprint)
        {
            // 전략 리스트 가져오기
            if (strategies != null)
                strategies.Clear();
            else strategies = new();
            List<Strategy> availableStrategies = CollectionUtility.FirstOrNull(blueprint.behaviourStrategies,
                                                                               beSt => beSt.behaviour.GetBehaviourType() == typeof(Look))
                                                                               .strategies;
            if (strategies == null)
            {
                Debug.Log("이 Actor는 Look 행동이 없습니다.");
                return;
            }
            else
            {
                foreach (var strategy in availableStrategies)
                {
                    if (strategy.isActive)
                        strategies.Add(strategy);
                }
            }
        }*/
#endregion