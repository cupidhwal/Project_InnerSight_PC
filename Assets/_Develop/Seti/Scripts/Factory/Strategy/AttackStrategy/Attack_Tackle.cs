using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Seti
{
    public class Attack_Tackle : Attack_Base
    {
        // 필드
        #region Variables
        CancellationTokenSource atkToken;   // 비동기 메서드 - Tackle 강제 종료용 토큰
        #endregion

        // 오버라이드
        #region Override
        public override void Attack()
        {
            base.Attack();

            Debug.Log("몬스터 돌진 공격");

            atkToken = new CancellationTokenSource(1000);   // 1초 후 자동 취소
            Tackle(atkToken.Token);
        }

        public override void AttackExit()
        {
            base.AttackExit();
            atkToken?.Cancel();
        }
        #endregion

        // 메서드
        #region Methods
        private async void Tackle(CancellationToken token)
        {
            if (actor is not Enemy enemy)
            {
                Debug.Log($"Attack_Tackle은 Enemy만 사용할 수 있습니다.");
                return;
            }
            if (!enemy.Player) return;

            try
            {
                // 공격 방향
                Vector3 atkDir = enemy.Player.transform.position - enemy.transform.position;

                // 축적
                enemy.Condition.IsAttack = true;
                float slamBack = 0f;
                float speed_slamBack = 0f;
                while (slamBack < 0.3f)
                {
                    if (token.IsCancellationRequested) return;
                    speed_slamBack = Mathf.Lerp(speed_slamBack, 3, 10 * Time.deltaTime);
                    enemy.transform.Translate(-speed_slamBack * Time.deltaTime * atkDir, Space.World);
                    slamBack += Time.deltaTime;
                    await Task.Delay((int)(Time.deltaTime * 1000), token);
                }

                // 돌진
                float slamFront = 0f;
                while (slamFront < 0.05f)
                {
                    if (token.IsCancellationRequested) return;

                    enemy.transform.Translate(30 * Time.deltaTime * atkDir, Space.World);
                    slamFront += Time.deltaTime;
                    await Task.Delay((int)(Time.deltaTime * 1000), token);  // 토큰 전달
                }

                // 반동
                float slamRebound = 0f;
                while (slamRebound < 0.1f)
                {
                    if (token.IsCancellationRequested) return;

                    enemy.transform.Translate(-5 * Time.deltaTime * atkDir, Space.World);
                    slamRebound += Time.deltaTime;
                    await Task.Delay((int)(Time.deltaTime * 1000), token);  // 토큰 전달
                }

                await Task.Delay(500);
                enemy.Condition.IsAttack = false;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("몬스터 돌진 공격 취소");
            }
        }
        #endregion
    }
}