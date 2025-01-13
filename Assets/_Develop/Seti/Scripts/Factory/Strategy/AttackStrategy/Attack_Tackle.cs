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
        CancellationTokenSource cts;    // 비동기 메서드 강제 종료용 토큰
        #endregion

        // 속성
        #region Properties
        public bool CanHit { get; private set; } = false;
        #endregion

        // 오버라이드
        #region Override
        public override void Attack()
        {
            base.Attack();

            Debug.Log("몬스터 돌진 공격");

            cts = new CancellationTokenSource(1000);    // 1초 후 자동 취소
            Tackle(cts.Token);
        }

        public override void AttackExit()
        {
            base.AttackExit();
            cts?.Cancel();
        }
        #endregion

        // 메서드
        private async void Tackle(CancellationToken token)
        {
            //if (rb == null) return;
            if (actor is not Enemy enemy)
            {
                Debug.Log($"Attack_Tackle은 Enemy만 사용할 수 있습니다.");
                return;
            }

            // 공격 방향
            Vector3 atkDir = enemy.Player.transform.position - actor.transform.position;
            actor.transform.LookAt(enemy.Player.transform.position);

            try
            {
                float slamBack = 0f;
                while (slamBack < 0.45f)
                {
                    if (token.IsCancellationRequested) return;

                    actor.transform.Translate(-2 * Time.deltaTime * atkDir, Space.World);
                    slamBack += Time.deltaTime;
                    await Task.Delay((int)(Time.deltaTime * 1000), token);
                }

                CanHit = true;
                float slamFront = 0f;
                while (slamFront < 0.05f)
                {
                    if (token.IsCancellationRequested) return;

                    actor.transform.Translate(30 * Time.deltaTime * atkDir, Space.World);
                    slamFront += Time.deltaTime;
                    await Task.Delay((int)(Time.deltaTime * 1000), token);  // 토큰 전달
                }
                CanHit = false;

                float slamRebound = 0f;
                while (slamRebound < 0.05f)
                {
                    if (token.IsCancellationRequested) return;

                    actor.transform.Translate(-5 * Time.deltaTime * atkDir, Space.World);
                    slamRebound += Time.deltaTime;
                    await Task.Delay((int)(Time.deltaTime * 1000), token);  // 토큰 전달
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("몬스터 돌진 공격 취소");
            }
        }
    }
}