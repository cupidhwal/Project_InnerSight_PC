using System.Threading.Tasks;
using UnityEngine;

namespace Seti
{
    public class Move_Dash : Move_Base
    {
        // 필드
        #region Variables
        private bool isDash = false;
        private Vector2 dir = Vector2.zero;
        private Vector3 moveDirection = Vector3.zero;

        private const float dashDuration = 0.1f;
        #endregion

        // 오버라이드
        #region Override
        protected override async void QuaterView_Move(Vector2 moveInput)
        {
            //if (rb == null) return;
            if (actor is not Player player)
            {
                Debug.Log("Move_Dash는 Player만 사용할 수 있습니다.");
                return;
            }

            if (!isDash)    // 대시 중이 아닐 때에만 방향 갱신
            {
                dir = MoveDirection(moveInput);
                moveDirection = (moveInput == Vector2.zero) ?
                                Quaternion.Euler(0f, -45f, 0f) * player.transform.forward :
                                new(dir.x, 0, dir.y);
                isDash = true;
            }
            Vector3 move = speed * Time.deltaTime * moveDirection.normalized;
            Vector3 QuaterView = Quaternion.Euler(0f, 45f, 0f) * move;

            float elapsedTime = 0f;

            // 초기 속도 설정
            float currentSpeed = 0f;
            while (elapsedTime < dashDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / dashDuration;

                // Ease In-Out 적용
                currentSpeed = Mathf.Lerp(currentSpeed, player.Dash_Speed, Mathf.SmoothStep(0f, 1f, t * t));
                player.transform.Translate(currentSpeed * Time.deltaTime * QuaterView, Space.World);

                await Task.Delay((int)(Time.deltaTime * 1000));
            }
        }
        #endregion

        // 메서드
        #region Methods
        public void MoveExit()
        {
            isDash = false;
            dir = Vector2.zero;
            moveDirection = Vector3.zero;
        }
        #endregion
    }
}