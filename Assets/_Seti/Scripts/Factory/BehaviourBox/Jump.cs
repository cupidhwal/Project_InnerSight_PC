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
        // �ʵ�
        #region Variables
        [SerializeReference]
        private List<Strategy> strategies;
        private IJumpStrategy currentStrategy;
        #endregion

        // �������̽�
        #region Interface
        // ���׷��̵�
        public void Upgrade(float increment)
        {

        }

        // �ʱ�ȭ
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

            // �ʱ� ���� ����
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
                //Debug.LogWarning("Jump ������ ���� �ʱ� ������ �������� ���߽��ϴ�.");
                ChangeStrategy(null);
            }
        }

        public Type GetBehaviourType() => typeof(Jump);
        public Type GetStrategyType() => typeof(IJumpStrategy);

        // �ൿ ���� ����
        public void SetStrategies(IEnumerable<Strategy> strategies)
        {
            this.strategies = strategies.ToList(); // ���޹��� ���� ����Ʈ ����
        }

        // �ൿ ���� ����
        public void ChangeStrategy(Type strategyType)
        {
            var moveStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy.GetType() == strategyType);
            if (moveStrategy != null)
            {
                currentStrategy = moveStrategy.strategy as IJumpStrategy;
            }
        }
        #endregion

        // �̺�Ʈ �ڵ鷯
        #region Event Handlers
        public void OnJumpStarted(InputAction.CallbackContext _)
        {
            currentStrategy?.Jump();
        }
        #endregion
    }
}