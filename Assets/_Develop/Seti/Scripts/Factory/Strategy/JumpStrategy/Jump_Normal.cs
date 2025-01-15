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
            if (!actor.ActorCondition) return;

            Condition_Actor state = actor.ActorCondition;
            if (state.IsGrounded)
                rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}