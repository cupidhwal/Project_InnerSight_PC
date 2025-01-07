using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Jump Behaviour¿« Strategy - Normal
    /// </summary>
    public class Jump_Normal : Jump_Base
    {
        public override void Jump()
        {
            if (!actor.ActorState) return;

            State_Common state = actor.ActorState as State_Common;
            if (state.IsGrounded)
                rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}