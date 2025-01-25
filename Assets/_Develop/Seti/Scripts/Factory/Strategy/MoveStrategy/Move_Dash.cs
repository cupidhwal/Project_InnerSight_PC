using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Seti
{
    public class Move_Dash : Move_Base
    {
        // 필드
        #region Variables
        CancellationTokenSource dashToken;
        Player player;

        private bool isDash = false;
        private Vector2 dir = Vector2.zero;
        private Vector3 moveDirection = Vector3.zero;
        #endregion

        // 오버라이드
        #region Override
        protected override void QuaterView_Move(Vector2 moveInput)
        {
            //if (rb == null) return;
            if (actor is not Player player)
            {
                Debug.Log("Move_Dash는 Player만 사용할 수 있습니다.");
                return;
            }
            if (this.player == null)
            {
                this.player = player;
            }

            // 대시 후 자동 종료
            int dashDuration = (int)(player.Dash_Duration * 1000);
            dashToken = new CancellationTokenSource(dashDuration);
            Dash(moveInput, dashToken.Token);
        }
        #endregion

        // 메서드
        #region Methods
        private async void Dash(Vector2 moveInput, CancellationToken token)
        {
            try
            {
                if (!isDash)    // 대시 중이 아닐 때에만 방향 갱신
                {
                    dir = MoveDirection(moveInput);
                    moveDirection = (moveInput == Vector2.zero) ?
                                    Quaternion.Euler(0f, -45f, 0f) * player.transform.forward :
                                    new(dir.x, 0, dir.y);
                    isDash = true;
                }
                Vector3 QuaterView = Quaternion.Euler(0f, 45f, 0f) * moveDirection.normalized;

                // 초기 속도 설정
                float elapsedTime = 0f;
                float currentSpeed = 0f;
                while (actor.Condition.InAction && elapsedTime < player.Dash_Duration)
                {
                    if (token.IsCancellationRequested) return;

                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / player.Dash_Duration;

                    // Ease In-Out 적용
                    currentSpeed = elapsedTime > (player.Dash_Duration / 2f) ? Mathf.Lerp(currentSpeed, player.Dash_Speed, Mathf.SmoothStep(0f, 1f, t)) : 0f;
                    player.transform.Translate(currentSpeed * Time.deltaTime * QuaterView, Space.World);

                    await Task.Delay((int)(Time.deltaTime * 1000));
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("대시 끝");
            }
        }

        public void MoveExit()
        {
            isDash = false;
            dir = Vector2.zero;
            moveDirection = Vector3.zero;
        }
        #endregion
    }
}