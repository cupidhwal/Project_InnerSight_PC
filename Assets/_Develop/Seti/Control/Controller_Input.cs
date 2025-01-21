using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// ControlType.Input Controller
    /// </summary>
    public class Controller_Input : Controller_Base, IController
    {
        // 필드
        #region Variables
        private InputSystem_Actions control;
        #endregion

        // 인터페이스
        #region Interface
        public Type GetControlType() => typeof(Control_Input);
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Update()
        {
            base.Update();

            if (behaviourMap.TryGetValue(typeof(Look), out var lookBehaviour))
            {
                Look look = lookBehaviour as Look;
                look?.Update();
            }
        }

        protected override void Awake()
        {
            // 초기화
            base.Awake();
            control = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            // 입력 이벤트 연결
            BindInputEvents();
            control.Enable();
        }

        private void OnDisable()
        {
            // 입력 이벤트 해제
            UnbindInputEvents();
            control.Disable();
        }
        #endregion

        // 메서드
        #region Methods
        private void BindInputEvents()
        {
            // Look 행동 이벤트 바인딩
            if (behaviourMap.TryGetValue(typeof(Look), out var lookBehaviour))
            {
                // 전략을 포함하고 있는지 확인
                if (lookBehaviour is Look look && look.HasStrategy<Look_Normal>())
                {
                    control.Player.Look.performed += look.OnLookPerformed;
                    control.Player.Look.canceled += look.OnLookCanceled;
                }
            }

            // Move 행동 이벤트 바인딩
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                // 전략을 포함하고 있는지 확인
                if (moveBehaviour is Move move)
                {
                    if (move.HasStrategy<Move_Normal>())
                    {
                        control.Player.Move.performed += move.OnMovePerformed;
                        control.Player.Move.canceled += move.OnMoveCanceled;
                    }
                    if (move.HasStrategy<Move_Dash>())
                    {
                        control.Player.Dash.started += move.OnDashStarted;
                    }
                    if (move.HasStrategy<Move_Run>())
                    {
                        //control.Player.Run.started += move.OnRunStarted;
                        //control.Player.Run.canceled += move.OnRunCanceled;
                    }
                }
            }

            // Jump 행동 이벤트 바인딩
            if (behaviourMap.TryGetValue(typeof(Jump), out var jumpBehaviour))
            {
                Jump jump = jumpBehaviour as Jump;
                control.Player.Jump.started += jump.OnJumpStarted;
            }

            // Attack 행동 이벤트 바인딩
            if (behaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
            {
                if (attackBehaviour is Attack attack)
                {
                    if (attack.HasStrategy<Attack_Normal>())
                    {
                        control.Player.Attack.started += attack.OnAttackStarted;
                        control.Player.Attack.canceled += attack.OnAttackCanceled;
                    }
                    if (attack.HasStrategy<Attack_Weapon>())
                    {
                        control.Player.Weapon.started += attack.OnWeaponStarted;
                        control.Player.Weapon.canceled += attack.OnWeaponCanceled;
                    }
                    if (attack.HasStrategy<Attack_Magic>())
                    {
                        control.Player.Magic.started += attack.OnMagicStarted;
                        control.Player.Magic.canceled += attack.OnMagicCanceled;
                    }
                }
            }
        }

        private void UnbindInputEvents()
        {
            // Look 행동 이벤트 해제
            if (behaviourMap.TryGetValue(typeof(Look), out var lookBehaviour))
            {
                Look look = lookBehaviour as Look;
                control.Player.Look.performed -= look.OnLookPerformed;
                control.Player.Look.canceled -= look.OnLookCanceled;
            }

            // Move 행동 이벤트 해제
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                Move move = moveBehaviour as Move;
                control.Player.Move.performed -= move.OnMovePerformed;
                control.Player.Move.canceled -= move.OnMoveCanceled;
                control.Player.Dash.started -= move.OnDashStarted;
                //control.Player.Run.started -= move.OnRunStarted;
                //control.Player.Run.canceled -= move.OnRunCanceled;
            }

            // Jump 행동 이벤트 해제
            if (behaviourMap.TryGetValue(typeof(Jump), out var jumpBehaviour))
            {
                Jump jump = jumpBehaviour as Jump;
                control.Player.Jump.started -= jump.OnJumpStarted;
            }

            // Attack 행동 이벤트 해제
            if (behaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
            {
                Attack attack = attackBehaviour as Attack;
                control.Player.Attack.started -= attack.OnAttackStarted;
                control.Player.Attack.canceled -= attack.OnAttackCanceled;
                control.Player.Weapon.started -= attack.OnWeaponStarted;
                control.Player.Weapon.canceled -= attack.OnWeaponCanceled;
                control.Player.Magic.started -= attack.OnMagicStarted;
                control.Player.Magic.canceled -= attack.OnMagicCanceled;
            }
        }
        #endregion
    }
}