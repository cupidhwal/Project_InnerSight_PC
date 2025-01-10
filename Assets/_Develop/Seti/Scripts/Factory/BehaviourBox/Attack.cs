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
        private enum StrategyType
        {
            Normal,
            Magic,
            Weapon,
            NULL
        }

        // 필드
        #region Variables
        // 공격력
        private float power_Normal = 10f;
        private const float power_Normal_Default = 10f;

        // 전략 관리
        private Actor actor;
        [SerializeReference]
        private List<Strategy> strategies;
        private IAttackStrategy currentStrategy;

        // 제어 관리
        private StrategyType currentType;
        private State<Controller_FSM> currentState;
        #endregion

        // 인터페이스
        #region Interface
        // 업그레이드
        public void Upgrade(float increment)
        {
            power_Normal += increment * power_Normal_Default / 100;
            Initialize(actor);
        }

        // 초기화
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

            // 초기 전략 설정
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
                //Debug.LogWarning("Move 전략이 없어 초기 전략을 설정하지 못했습니다.");
                ChangeStrategy(null);
            }
        }

        public Type GetBehaviourType() => typeof(Attack);
        public Type GetStrategyType() => typeof(IAttackStrategy);

        // 행동 전략 설정
        public void SetStrategies(IEnumerable<Strategy> strategies)
        {
            this.strategies = strategies.ToList(); // 전달받은 전략 리스트 저장
        }

        // 행동 전략 변경
        public void ChangeStrategy(Type strategyType)
        {
            var attackStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy.GetType() == strategyType);
            if (attackStrategy != null)
            {
                currentStrategy = attackStrategy.strategy as IAttackStrategy;
            }
        }

        private void SwitchStrategy(StrategyType attackStrategies)
        {
            switch (attackStrategies)
            {
                case StrategyType.Normal:
                    currentType = StrategyType.Normal;
                    ChangeStrategy(typeof(Attack_Normal));
                    break;

                case StrategyType.Magic:
                    currentType = StrategyType.Magic;
                    ChangeStrategy(typeof(Attack_Magic));
                    break;

                case StrategyType.Weapon:
                    currentType = StrategyType.Weapon;
                    ChangeStrategy(typeof(Attack_Weapon));
                    break;
            }
        }
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        #region Controller_Input
        public void OnAttackStarted(InputAction.CallbackContext _) => OnAttack(true);

        public void OnAttackCanceled(InputAction.CallbackContext _) => OnAttack(false);

        public void OnSkillStarted(InputAction.CallbackContext _)
        {
            SwitchStrategy(StrategyType.Weapon);
            currentStrategy?.Attack();
        }

        public void OnSkillCanceled(InputAction.CallbackContext _)
        {
            currentStrategy?.AttackExit();
            SwitchStrategy(StrategyType.Normal);
        }

        public void OnMagicStarted(InputAction.CallbackContext context)
        {
            SwitchStrategy(StrategyType.Magic);

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
            SwitchStrategy(StrategyType.Normal);
        }
        #endregion

        #region Controller_FSM
        public void FSM_AttackInput() => OnAttack();
        public void FSM_AttackSwitch(State<Controller_FSM> state)
        {
            // FSM 상태에 따라 동작 제어
            currentState = state;
            switch (currentState)
            {
                case Enemy_State_Attack:
                    SwitchStrategy(StrategyType.Normal);
                    break;

                default:
                    SwitchStrategy(StrategyType.NULL);
                    break;
            }
        }
        #endregion
        #endregion

        // 메서드
        #region Methods
        private void OnAttack(bool isAttack = true)
        {
            if (isAttack)
            {
                if (currentType != StrategyType.Normal)
                    SwitchStrategy(StrategyType.Normal);
                currentStrategy?.Attack();
            }
            else currentStrategy?.AttackExit();
            actor.Controller_Animator.IsAttack = isAttack;
        }
        #endregion
    }
}