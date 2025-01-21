using UnityEngine;
using UnityEngine.InputSystem;

namespace MySampleEx
{
    /// <summary>
    /// 플레이어(Agent) 인풋 관리 클래스
    /// </summary>
    public class PlayerInputAgent : MonoBehaviour
    {
        #region Variables
        public bool Click { get; set; }
        public Vector2 MousePosition { get; private set; }
        #endregion

        #region NewInput SendMessage
        public void OnClick(InputValue value)
        {
            ClickInput(value.isPressed);
        }

        public void OnMousePosition(InputValue value)
        {
            MousePositionInput(value.Get<Vector2>());
        }
        #endregion

        public void ClickInput(bool newClickState)
        {
            Click = newClickState;
        }

        public void MousePositionInput(Vector2 newMousePosition)
        {
            MousePosition = newMousePosition;
        }
    }
}
