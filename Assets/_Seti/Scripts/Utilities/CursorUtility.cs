using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    // 마우스 커서의 활용을 제어하고 포인터의 Position을 계산하는 유틸리티
    public class CursorUtility
    {
        // 필드
        #region Variables
        // 단순 변수
        //private bool isCursorOn = false;

        // 복합 변수
        private Vector2 cursorPosition;

        // 클래스 컴포넌트
        private readonly Player player;
        #endregion

        // 속성
        #region Properties
        public Vector2 CursorPosition => cursorPosition;
        #endregion

        // 생성자
        #region Constructor
        public CursorUtility(Player player)
        {
            this.player = player;
        }
        #endregion

        // 이벤트 핸들러
        #region Event Handlers
        public void OnCursorSwitchStarted(InputAction.CallbackContext _)
        {
            /*if (player.PlayerUse.InventoryManager.IsOpenInventory) return;
            CursorSwitch(isCursorOn = !isCursorOn);*/
        }

        public void OnCursorSwitchCanceled(InputAction.CallbackContext _)
        {
            /*if (player.PlayerUse.InventoryManager.IsOpenInventory || !isCursorOn) return;
            CursorSwitch(isCursorOn = !isCursorOn);*/
        }

        public void OnCursorPositionPerformed(InputAction.CallbackContext context) => cursorPosition = context.ReadValue<Vector2>();
        #endregion

        // 메서드
        #region Methods
        // 마우스 커서 토글 메서드
        public void CursorSwitch(bool isOpen)
        {
            /*if (isOpen)
            {
                // 플레이어 시야 제어 잠금
                player.control.Player.Look.Disable();

                // 마우스 커서 포지션 컨트롤 입력 활성화
                player.control.Player.CursorPosition.Enable();

                // 마우스 커서 클릭 컨트롤 입력 활성화
                player.control.Player.CursorClick.Enable();

                // 마우스 커서 해제
                Cursor.lockState = CursorLockMode.Confined;
            }

            else
            {
                // 마우스 커서 잠금
                Cursor.lockState = CursorLockMode.Locked;

                // 마우스 커서 포지션 컨트롤 입력 비활성화
                player.control.Player.CursorPosition.Disable();

                // 마우스 커서 클릭 컨트롤 입력 비활성화
                player.control.Player.CursorClick.Disable();

                // 플레이어 시야 제어 해제
                player.control.Player.Look.Enable();

                isCursorOn = false;
            }*/
        }

        // 마우스 포인터 클릭으로 오브젝트를 감지하는 유틸리티
        public Transform CursorSelect()
        {
            Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 3.0f))
                return hit.collider.transform;
            else return null;
        }
        #endregion
    }
}