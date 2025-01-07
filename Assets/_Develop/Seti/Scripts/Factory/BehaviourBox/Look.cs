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
        // �ʵ�
        #region Variables
        [SerializeReference]
        private List<Strategy> strategies;
        private ILookStrategy currentStrategy;
        private Vector2 lookInput;
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
                ILookStrategy moveStrategy = mapping.strategy as ILookStrategy;
                switch (moveStrategy)
                {
                    case Look_Normal:
                        moveStrategy.Initialize(actor, 0.1f);
                        break;

                    /*case Look_KeepGoing:
                        moveStrategy.Initialize(actor, 0.1f);
                        break;*/
                }
            }

            // �ʱ� ���� ����
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
                //Debug.LogWarning("Look ������ ���� �ʱ� ������ �������� ���߽��ϴ�.");
                ChangeStrategy(null);
            }
        }

        public Type GetBehaviourType() => typeof(Look);
        public Type GetStrategyType() => typeof(ILookStrategy);

        // �ൿ ���� ����
        public void SetStrategies(IEnumerable<Strategy> strategies)
        {
            this.strategies = strategies.ToList(); // ���޹��� ���� ����Ʈ ����
        }

        // �ൿ ���� ����
        public void ChangeStrategy(Type strategyType)
        {
            var lookStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy.GetType() == strategyType);
            if (lookStrategy != null)
            {
                currentStrategy = lookStrategy.strategy as ILookStrategy;
            }
        }
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        public void FixedUpdate()
        {

        }

        public void LateUpdate()
        {

        }
        #endregion

        // �̺�Ʈ �ڵ鷯
        #region Event Handlers
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
    }
}

#region Dummy
/*public void SetStrategy(Blueprint_Actor blueprint)
        {
            // ���� ����Ʈ ��������
            if (strategies != null)
                strategies.Clear();
            else strategies = new();
            List<Strategy> availableStrategies = CollectionUtility.FirstOrNull(blueprint.behaviourStrategies,
                                                                               beSt => beSt.behaviour.GetBehaviourType() == typeof(Look))
                                                                               .strategies;
            if (strategies == null)
            {
                Debug.Log("�� Actor�� Look �ൿ�� �����ϴ�.");
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