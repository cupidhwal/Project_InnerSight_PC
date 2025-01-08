using UnityEngine;

namespace Seti
{
    public abstract class State_Actor : MonoBehaviour { }

    public class State_Common : State_Actor
    {
        // �ʵ�
        #region Variables
        public bool IsGrounded { get; set; }
        public bool IsAttack { get; set; }
        #endregion

        // �̺�Ʈ �޼���
        #region Event Methods
        // Collision �ø���
        #region OnCollision
        private void OnCollisionChange(Collision collision, bool groundedState)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                IsGrounded = groundedState;
            }
        }

        private void OnCollisionEnter(Collision collision) => OnCollisionChange(collision, true);
        private void OnCollisionStay(Collision collision) => OnCollisionChange(collision, true);
        private void OnCollisionExit(Collision collision) => OnCollisionChange(collision, false);
        #endregion
        #endregion
    }
}