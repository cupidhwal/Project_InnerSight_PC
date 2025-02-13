using System;
using System.Collections;
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
            Weapon,
            Magic,
            Tackle,
            NULL
        }

        // 필드
        #region Variables
        // 공격력
        [SerializeField]
        private float attack;

        // 전략 관리
        private Actor actor;
        [SerializeReference]
        private List<Strategy> strategies;
        private IAttackStrategy currentStrategy;

        // 제어 관리
        private Condition_Actor condition;
        //private StrategyType currentType;
        private State<Controller_FSM> currentState;

        // 스킬 애니메이션 관리
        public bool isSkillAttack = false;
        #endregion

        // 인터페이스
        #region Interface
        // 업그레이드
        public void Upgrade(float increment)
        {
            if (actor is not Player) return;

            attack = actor.Attack;
            attack += increment * actor.Attack_Default / 100;
            actor.Update_Attack(attack);
            Initialize(actor);
        }

        // 초기화
        public void Initialize(Actor actor)
        {
            this.actor = actor;
            condition = actor.Condition;

            foreach (var mapping in strategies)
            {
                IAttackStrategy attackStrategy = mapping.strategy as IAttackStrategy;
                switch (attackStrategy)
                {
                    case Attack_Normal:
                        attackStrategy.Initialize(actor, attack);
                        break;

                    case Attack_Magic:
                        attackStrategy.Initialize(actor);
                        break;

                    case Attack_Weapon:
                        attackStrategy.Initialize(actor);
                        break;

                    case Attack_Tackle:
                        attackStrategy.Initialize(actor);
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
                //Debug.LogWarning($"{} 전략이 없어 초기 전략을 설정하지 못했습니다.");
                ChangeStrategy(null);
            }
        }

        public Type GetBehaviourType() => typeof(Attack);
        public Type GetStrategyType() => typeof(IAttackStrategy);

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
            var attackStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy.GetType() == strategyType);
            if (attackStrategy != null)
            {
                currentStrategy = attackStrategy.strategy as IAttackStrategy;
            }
        }

        // 행동 전략 제어
        public void SwitchStrategy(State<Controller_FSM> state)
        {
            // FSM 상태에 따라 동작 제어
            currentState = state;
            switch (currentState)
            {
                case Enemy_State_Attack_Normal:
                    ChangeStrategy(typeof(Attack_Normal));
                    break;

                /*case Enemy_State_Attack_Weapon:
                    ChangeStrategy(typeof(Attack_Weapon));
                    break;*/

                case Enemy_State_Attack_Magic:
                    ChangeStrategy(typeof(Attack_Magic));
                    break;

                default:
                    ChangeStrategy(null);
                    break;
            }
        }
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        #region Controller_Input
        public void OnAttackStarted(InputAction.CallbackContext _) => OnAttack(true);
        public void OnAttackCanceled(InputAction.CallbackContext _) => OnAttack(false);
        public void OnWeaponStarted(InputAction.CallbackContext _) => OnMagic(true);
        public void OnWeaponCanceled(InputAction.CallbackContext _)
        {
            //OnMagic(false);
        }
        public void OnMagicStarted(InputAction.CallbackContext context)
        {
            string path = context.control.path;
            switch (path)
            {
                case "/Keyboard/1":
                    //Debug.Log("Magic 1");
                    break;

                case "/Keyboard/2":
                    //Debug.Log("Magic 2");
                    break;

                case "/Keyboard/3":
                    //Debug.Log("Magic 3");
                    break;

                case "/Keyboard/4":
                    //Debug.Log("Magic 4");
                    break;
            }

            //OnMagic(true);
        }
        public void OnMagicCanceled(InputAction.CallbackContext _)
        {
            //SwitchStrategy(StrategyType.Normal);
        }
        #endregion

        #region Controller_FSM
        public void FSM_AttackInput(bool isAttack) => OnAttack(isAttack);
        public void FSM_MagicInput(bool isMagic) => OnMagic(isMagic);
        #endregion
        #endregion

        // 메서드
        #region Methods
        public void OnAttack(bool isAttack = true)
        {
            if (!condition.InAction) return;

            ChangeStrategy(typeof(Attack_Normal));
            condition.IsAttack = isAttack;

            if (isSkillAttack)
                isSkillAttack = false;
        }
        public void OnAttackEnter() => currentStrategy?.Attack();

        public void OnMagic(bool isMagic = true)
        {
            if (!condition.InAction) return;

            if (isMagic)
            {
                if (actor is Player)
                {
                    if (isSkillAttack)
                    {
                        ChangeStrategy(typeof(Attack_Magic));
                        currentStrategy?.Attack();

                        condition.AttackPoint = Noah.RayManager.Instance.RayToScreen();
                        ChangeStrategy(typeof(Attack_Normal));

                        isSkillAttack = false;
                    }
                }
                else
                {
                    ChangeStrategy(typeof(Attack_Magic));
                    currentStrategy?.Attack();
                }
            }
        }
        #endregion
    }
}