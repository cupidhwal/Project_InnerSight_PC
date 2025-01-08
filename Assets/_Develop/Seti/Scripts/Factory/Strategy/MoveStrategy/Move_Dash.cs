using UnityEngine;

namespace Seti
{
    public class Move_Dash : Move_Base
    {
        private bool isDash = false;
        private Vector2 dir = Vector2.zero;
        private Vector3 moveDirection = Vector3.zero;

        public override void Move(Vector2 moveInput)
        {
            if (rb == null) return;
            if (!isDash)
            {
                dir = MoveDirection(moveInput);
                moveDirection = (moveInput == Vector2.zero) ?
                                Quaternion.Euler(0f, -45f, 0f) * actor.transform.forward :
                                new(dir.x, 0, dir.y);
                isDash = true;
            }
            Move(moveDirection);
        }
        public void MoveExit()
        {
            isDash = false;
            dir = Vector2.zero;
            moveDirection = Vector3.zero;
        }
    }
}