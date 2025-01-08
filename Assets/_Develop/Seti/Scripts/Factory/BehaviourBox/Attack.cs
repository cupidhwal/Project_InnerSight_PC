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
        // ���ݷ�
        private float power_Normal = 10f;
        private const float power_Normal_Default = 10f;

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
                        moveStrategy.Initialize(actor);
                        break;

                    case Attack_Weapon:
                        moveStrategy.Initialize(actor);
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

        public void OnAttackCanceled(InputAction.CallbackContext _)
        {
            currentStrategy?.AttackExit();
        }

        public void OnSkillStarted(InputAction.CallbackContext _)
        {
            SwitchStrategy(AttackStrategies.Weapon);
            currentStrategy?.Attack();
        }

        public void OnSkillCanceled(InputAction.CallbackContext _)
        {
            currentStrategy?.AttackExit();
            SwitchStrategy(AttackStrategies.Normal);
        }

        public void OnMagicStarted(InputAction.CallbackContext context)
        {
            SwitchStrategy(AttackStrategies.Magic);

            string path = context.control.path;
            switch (path)
            {
                case "/Keyboard/1":
                    Debug.Log("Magic 1");
                    break;

                case "/Keyboard/2":
                    Debug.Log("Magic 2");
                    break;

                case "/Keyboard/3":
                    Debug.Log("Magic 3");
                    break;

                case "/Keyboard/4":
                    Debug.Log("Magic 4");
                    break;
            }

            currentStrategy?.Attack();
        }
        
        public void OnMagicCanceled(InputAction.CallbackContext _)
        {
            currentStrategy?.AttackExit();
            SwitchStrategy(AttackStrategies.Normal);
        }
        #endregion
    }
}