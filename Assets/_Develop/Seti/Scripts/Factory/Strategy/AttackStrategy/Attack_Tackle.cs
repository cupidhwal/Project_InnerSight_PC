using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Seti
{
    public class Attack_Tackle : Attack_Base
    {
        CancellationTokenSource cts;

        // 오버라이드
        #region Override
        public override void Attack()
        {
            base.Attack();

            Debug.Log("몬스터 돌진 공격");

            Rigidbody rb_Enemy = actor.GetComponent<Rigidbody>();
            cts = new CancellationTokenSource(1200); // 1.2초 후 자동 취소
            Tackle(rb_Enemy, cts.Token);
        }

        public override void AttackExit()
        {
            base.AttackExit();
            cts?.Cancel();
        }
        #endregion

        // 메서드
        private async void Tackle(Rigidbody rb, CancellationToken token)
        {
            if (rb == null) return;

            try
            {
                float slamBack = 0f;
                while (slamBack < 0.9f)
                {
                    if (token.IsCancellationRequested) return;

                    rb.MovePosition(actor.transform.position - 0.5f * Time.fixedDeltaTime * actor.transform.forward);
                    slamBack += Time.fixedDeltaTime;
                    await Task.Delay(20, token); // 토큰 전달
                }

                float slamFront = 0f;
                while (slamFront < 0.2f)
                {
                    if (token.IsCancellationRequested) return;

                    rb.MovePosition(actor.transform.position + 10 * Time.fixedDeltaTime * actor.transform.forward);
                    slamFront += Time.fixedDeltaTime;
                    await Task.Delay(20, token); // 토큰 전달
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}