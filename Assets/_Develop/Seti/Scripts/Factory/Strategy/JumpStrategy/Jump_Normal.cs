using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Jump Behaviour의 Strategy - Normal
    /// </summary>
    public class Jump_Normal : Jump_Base
    {
        public override void Jump()
        {
            if (!actor.ActorState) return;

            Condition_Common state = actor.ActorState as Condition_Common;
            if (state.IsGrounded)
                rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}