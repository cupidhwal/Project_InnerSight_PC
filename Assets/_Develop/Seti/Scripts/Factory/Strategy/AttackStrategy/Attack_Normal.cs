using System.Collections;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 평타
    /// </summary>
    public class Attack_Normal : Attack_Base
    {
        // 오버라이드
        #region Override
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
                    actor.CoroutineExecutor(Attack_Sword());
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

        private IEnumerator Attack_Sword()
        {
            // 애니메이션의 Root Motion을 쓰지 않을 경우에만 실행
            if (actor.Controller_Animator.Animator.applyRootMotion) yield break;

            // Player의 검 공격, 조금씩 전진
            Player player = actor as Player;

            // 초기 속도 설정
            float elapsedTime = 0f;
            float atkDuration = 0.16f;
            float currentSpeed = player.Rate_Movement_Default * player.AttackProgressive;
            while (actor.Condition.InAction && elapsedTime < atkDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / atkDuration;

                // Ease In-Out 적용
                currentSpeed = Mathf.Lerp(currentSpeed, 0, Mathf.SmoothStep(0f, 1f, t));
                player.transform.Translate(currentSpeed * Time.deltaTime * player.transform.forward, Space.World);

                yield return null;
            }

            yield break;
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
            //Debug.Log("평타: Null");
        }
        #endregion
    }
}