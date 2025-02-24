using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    public class Dash : IBehaviour
    {
        // 필드
        #region Variables
        // 행동 관리
        private Player player;
        private Condition_Player condition;

        private bool isDashing = false;
        private Move move;
        private Vector3 moveDirection = Vector3.zero;
        #endregion

        // 인터페이스
        #region Interface
        // 초기화
        public void Initialize(Actor actor)
        {
            if (actor is not Player)
            {
                Debug.Log("Dash Behaviour는 Player만 사용할 수 있습니다.");
                return;
            }

            player = actor as Player;
            condition = actor.Condition as Condition_Player;
        }

        public Type GetBehaviourType() => typeof(Dash);
        #endregion

        // 라이프 사이클
        #region Life Cycle
        public void Update()
        {
            if (!player.Condition.InAction) return;
        }
        #endregion

        // 컨트롤러
        #region Controllers
        public void OnDashStarted(InputAction.CallbackContext _) => OnDash();
        #endregion

        // 메서드
        #region Methods
        private void OnDash()
        {
            Condition_Player condition_Player = player.Condition as Condition_Player;

            if (condition_Player.InAction && condition_Player.IsGrounded && condition_Player.CanDash)
                player.CoroutineExecutor(Dash_Cor());
        }
        #endregion

        // 유틸리티
        #region Utilities
        private IEnumerator Dash_Cor()
        {
            if (player.Controller.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                move = moveBehaviour as Move;
            }
            else
            {
                Debug.Log("Dash Behaviour는 Move Behaviour와 함께 사용해야 합니다.");
                yield break;
            }

            // 대시 중 충돌 무시
            player.CoroutineExecutor(GameUtility.Timer_Collision(player.transform,
                                                                 LayerMask.NameToLayer("Actor"),
                                                                 player.Dash_Duration));

            // 대시 시작
            condition.IsDash = true;
            condition.CanDash = false;

            // Damagable 컴포넌트가 있다면 대시 중 무적
            if (player.TryGetComponent<Damagable>(out var damagable))
                damagable.isDashInvulnerable = true;

            // 대시 기능
            player.CoroutineExecutor(Dash_Excute(move.MoveInput));

            // 대시 끝
            yield return new WaitForSeconds(player.Dash_Duration);
            condition.IsDash = false;

            // 대시 사용 가능
            yield return new WaitForSeconds(player.Dash_Cooldown - player.Dash_Duration);
            condition.CanDash = true;

            yield break;
        }

        private IEnumerator Dash_Excute(Vector2 moveInput)
        {
            // 대시 기능
            if (!isDashing)    // 대시 중이 아닐 때에만 방향 갱신
            {
                moveDirection = (moveInput == Vector2.zero) ?
                                Quaternion.Euler(0f, -45f, 0f) * player.transform.forward :
                                new(moveInput.x, 0, moveInput.y);

                // 진행 방향으로 회전
                //player.transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);

                isDashing = true;
            }
            Vector3 QuaterView = Quaternion.Euler(0f, 45f, 0f) * moveDirection.normalized;

            // 초기 속도 설정
            float elapsedTime = 0f;
            float currentSpeed = 0f;
            while (condition.InAction && elapsedTime < player.Dash_Duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / player.Dash_Duration;

                // Ease In-Out 적용
                currentSpeed = elapsedTime > (player.Dash_Duration / 4f) ? Mathf.Lerp(currentSpeed, player.Dash_Speed, Mathf.SmoothStep(0f, 1f, t)) : 0f;
                player.transform.Translate(currentSpeed * Time.deltaTime * QuaterView, Space.World);

                if (condition.IsDash)
                {
                    // 진행 방향으로 회전
                    Quaternion targetRotation = Quaternion.LookRotation(Quaternion.Euler(0f, 45f, 0f) * moveDirection, Vector3.up);
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 100f * Time.deltaTime);
                }

                yield return null;
            }

            isDashing = false;
            yield break;
        }
        #endregion
    }
}