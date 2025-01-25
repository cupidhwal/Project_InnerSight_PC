using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Seti
{
    public class Attack_Normal : Attack_Base
    {
        // 필드
        CancellationTokenSource atkToken;
        float atkDuration = 0.16f;

        // 추상화
        #region Abstract
        public override void Attack()
        {
            base.Attack();
            Attack_WithWeapon();
        }
        #endregion

        // 메서드
        #region Methods
        private void Attack_WithWeapon()
        {
            switch (condition.CurrentWeaponType)
            {
                case Condition_Actor.WeaponType.Sword:
                    atkToken = new(200);
                    Attack_Sword(atkToken.Token);
                    break;

                case Condition_Actor.WeaponType.Staff:
                    Attack_Staff();
                    break;

                case Condition_Actor.WeaponType.Fist:
                    Attack_Fist();
                    break;

                case Condition_Actor.WeaponType.Bow:
                    Attack_Bow();
                    break;

                default:
                    Attack_Null();
                    break;
            }
        }

        private async void Attack_Sword(CancellationToken token)
        {
            // 애니메이션의 Root Motion을 쓰지 않을 경우에만 실행
            if (actor.Controller_Animator.Animator.applyRootMotion) return;

            // Player의 검 공격, 조금씩 전진
            try
            {
                Player player = actor as Player;

                // 초기 속도 설정
                float elapsedTime = 0f;
                float currentSpeed = player.Rate_Movement_Default * 2f;
                while (actor.Condition.InAction && elapsedTime < atkDuration)
                {
                    if (token.IsCancellationRequested) return;

                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / atkDuration;

                    // Ease In-Out 적용
                    currentSpeed = Mathf.Lerp(currentSpeed, 0, Mathf.SmoothStep(0f, 1f, t));
                    player.transform.Translate(currentSpeed * Time.deltaTime * player.transform.forward, Space.World);

                    await Task.Delay((int)(Time.deltaTime * 1000));
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("기본 공격 끝");
            }
        }

        private void Attack_Staff()
        {
            Debug.Log("평타: Staff");
        }

        private void Attack_Fist()
        {
            Debug.Log("평타: Fist");
        }

        private void Attack_Bow()
        {
            Debug.Log("평타: Bow");
        }

        private void Attack_Null()
        {
            // Enemy의 맨손 공격
            Debug.Log("평타: Null");
        }
        #endregion
    }
}