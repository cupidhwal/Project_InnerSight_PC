using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// Attack Behaviour
    /// </summary>
    public class Attack : IBehaviour, IHasStrategy
    {
        private enum AttackStrategies
        {
            Normal,
            Magic,
            Weapon,
        }

        // �ʵ�
        #region Variables
        // �ΰ��� - �޸���
        private float power_Normal = 10f;
        private const float power_Normal_Default = 10f;

        // ����
        private const float power_Magic = 50f;
        private const float power_Weapon = 30f;

        // ���� ����
        private Actor actor;
        [SerializeReference]
        private List<Strategy> strategies;
        private IAttackStrategy currentStrategy;

        // ���� ����
        private AttackStrategies currentType;
        #endregion

        // �������̽�
        #region Interface
        // ���׷��̵�
        public void Upgrade(float increment)
        {
            power_Normal += increment * power_Normal_Default / 100;
            Initialize(actor);
        }

        // �ʱ�ȭ
        public void Initialize(Actor actor)
        {
            this.actor = actor;
            foreach (var mapping in strategies)
            {
                IAttackStrategy moveStrategy = mapping.strategy as IAttackStrategy;
                switch (moveStrategy)
                {
                    case Attack_Normal:
                        moveStrategy.Initialize(actor, power_Normal);
                        break;

                    case Attack_Magic:
                        moveStrategy.Initialize(actor, power_Magic);
                        break;

                    case Attack_Weapon:
                        moveStrategy.Initialize(actor, power_Weapon);
                        break;
                }
            }

            // �ʱ� ���� ����
            var defaultStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy is Attack_Normal);
            if (defaultStrategy != null)
            {
                ChangeStrategy(typeof(Attack_Normal));
            }
            else if (strategies.Count > 0)
            {
                ChangeStrategy(strategies[0].strategy.GetType());
            }
            else
            {
                //Debug.LogWarning("Move ������ ���� �ʱ� ������ �������� ���߽��ϴ�.");
                ChangeStrategy(null);
            }
        }

        public Type GetBehaviourType() => typeof(Attack);
        public Type GetStrategyType() => typeof(IAttackStrategy);

        // �ൿ ���� ����
        public void SetStrategies(IEnumerable<Strategy> strategies)
        {
            this.strategies = strategies.ToList(); // ���޹��� ���� ����Ʈ ����
        }

        // �ൿ ���� ����
        public void ChangeStrategy(Type strategyType)
        {
            var attackStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy.GetType() == strategyType);
            if (attackStrategy != null)
            {
                currentStrategy = attackStrategy.strategy as IAttackStrategy;
            }
        }

        private void SwitchStrategy(AttackStrategies attackStrategies)
        {
            switch (attackStrategies)
            {
                case AttackStrategies.Normal:
                    currentType = AttackStrategies.Normal;
                    ChangeStrategy(typeof(Attack_Normal));
                    break;

                case AttackStrategies.Magic:
                    currentType = AttackStrategies.Magic;
                    ChangeStrategy(typeof(Attack_Magic));
                    break;

                case AttackStrategies.Weapon:
                    currentType = AttackStrategies.Weapon;
                    ChangeStrategy(typeof(Attack_Weapon));
                    break;
            }
        }
        #endregion

        // �̺�Ʈ �ڵ鷯
        #region Event Handlers
        public void OnAttackStarted(InputAction.CallbackContext _)
        {
            if (currentType != AttackStrategies.Normal)
                SwitchStrategy(AttackStrategies.Normal);
            currentStrategy?.Attack();
        }

        public void OnSkillStarted(InputAction.CallbackContext context)
        {
            SwitchStrategy(AttackStrategies.Weapon);
            currentStrategy?.Attack();
        }

        public void OnSkillCanceled(InputAction.CallbackContext context)
        {
            SwitchStrategy(AttackStrategies.Normal);
        }

        public void OnMagicStarted(InputAction.CallbackContext context)
        {
            SwitchStrategy(AttackStrategies.Magic);
            currentStrategy?.Attack();
        }
        
        public void OnMagicCanceled(InputAction.CallbackContext context)
        {
            SwitchStrategy(AttackStrategies.Normal);
        }
        #endregion
    }
}