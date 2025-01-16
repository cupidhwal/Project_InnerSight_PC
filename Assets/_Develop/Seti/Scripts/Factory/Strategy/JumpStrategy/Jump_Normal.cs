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
            if (!actor.Condition) return;

            Condition_Actor state = actor.Condition;
            if (state.IsGrounded)
                rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}