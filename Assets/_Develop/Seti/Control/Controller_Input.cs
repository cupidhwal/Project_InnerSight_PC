using System;

namespace Seti
{
    /// <summary>
    /// ControlType.Input Controller
    /// </summary>
    public class Controller_Input : Controller_Base, IController
    {
        // �ʵ�
        #region Variables
        private InputSystem_Actions control;
        #endregion

        // �������̽�
        #region Interface
        public Type GetControlType() => typeof(Control_Input);
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Awake()
        {
            // �ʱ�ȭ
            control = new InputSystem_Actions();
            Initialize();
        }

        private void OnEnable()
        {
            // �Է� �̺�Ʈ ����
            BindInputEvents();
            control.Enable();
        }

        private void OnDisable()
        {
            // �Է� �̺�Ʈ ����
            UnbindInputEvents();
            control.Disable();
        }
        #endregion

        // �޼���
        #region Methods
        private void BindInputEvents()
        {
            // Look �ൿ �̺�Ʈ ���ε�
            if (behaviourMap.TryGetValue(typeof(Look), out var lookBehaviour))
            {
                Look look = lookBehaviour as Look;
                control.Player.Look.performed += look.OnLookPerformed;
                control.Player.Look.canceled += look.OnLookCanceled;
            }

            // Move �ൿ �̺�Ʈ ���ε�
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                Move move = moveBehaviour as Move;
                control.Player.Move.performed += move.OnMovePerformed;
                control.Player.Move.canceled += move.OnMoveCanceled;
                //control.Player.Run.started += move.OnRunStarted;
                //control.Player.Run.canceled += move.OnRunCanceled;
            }

            // Jump �ൿ �̺�Ʈ ���ε�
            if (behaviourMap.TryGetValue(typeof(Jump), out var jumpBehaviour))
            {
                Jump jump = jumpBehaviour as Jump;
                control.Player.Jump.started += jump.OnJumpStarted;
            }
        }

        private void UnbindInputEvents()
        {
            // Look �ൿ �̺�Ʈ ����
            if (behaviourMap.TryGetValue(typeof(Look), out var lookBehaviour))
            {
                Look look = lookBehaviour as Look;
                control.Player.Look.performed -= look.OnLookPerformed;
                control.Player.Look.canceled -= look.OnLookCanceled;
            }

            // Move �ൿ �̺�Ʈ ����
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                Move move = moveBehaviour as Move;
                control.Player.Move.performed -= move.OnMovePerformed;
                control.Player.Move.canceled -= move.OnMoveCanceled;
                //control.Player.Run.started -= move.OnRunStarted;
                //control.Player.Run.canceled -= move.OnRunCanceled;
            }

            // Jump �ൿ �̺�Ʈ ����
            if (behaviourMap.TryGetValue(typeof(Jump), out var jumpBehaviour))
            {
                Jump jump = jumpBehaviour as Jump;
                control.Player.Jump.started -= jump.OnJumpStarted;
            }
        }
        #endregion
    }
}