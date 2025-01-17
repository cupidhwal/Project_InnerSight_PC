using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// Jump Function
    /// </summary>
    public class Jump : IBehaviour, IHasStrategy
    {
        // 필드
        #region Variables
        [SerializeReference]
        private List<Strategy> strategies;
        private IJumpStrategy currentStrategy;
        #endregion

        // 인터페이스
        #region Interface
        // 업그레이드
        public void Upgrade(float increment)
        {

        }

        // 초기화
        public void Initialize(Actor actor)
        {
            foreach (var mapping in strategies)
            {
                IJumpStrategy jumpStrategy = mapping.strategy as IJumpStrategy;
                switch (jumpStrategy)
                {
                    case Jump_Normal:
                        jumpStrategy.Initialize(actor, 350);
                        break;

                    case Jump_Boots:
                        jumpStrategy.Initialize(actor, 2000);
                        break;
                }
            }

            // 초기 전략 설정
            var defaultStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy is Jump_Normal);
            if (defaultStrategy != null)
            {
                ChangeStrategy(typeof(Jump_Normal));
            }
            else if (strategies.Count > 0)
            {
                ChangeStrategy(strategies[0].strategy.GetType());
            }
            else
            {
                //Debug.LogWarning("Jump 전략이 없어 초기 전략을 설정하지 못했습니다.");
                ChangeStrategy(null);
            }
        }

        public Type GetBehaviourType() => typeof(Jump);
        public Type GetStrategyType() => typeof(IJumpStrategy);

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
                currentStrategy = moveStrategy.strategy as IJumpStrategy;
            }
        }
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        public void OnJumpStarted(InputAction.CallbackContext _)
        {
            currentStrategy?.Jump();
        }
        #endregion
    }
}