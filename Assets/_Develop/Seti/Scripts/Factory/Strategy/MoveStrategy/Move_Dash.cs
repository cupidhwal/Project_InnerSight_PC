using UnityEngine;

namespace Seti
{
    public class Move_Dash : Move_Base
    {
        private bool isDash = false;
        private Vector2 dir = Vector2.zero;
        private Vector3 moveDirection = Vector3.zero;

        protected override void QuaterView_Move(Vector2 moveInput)
        {
            if (rb == null) return;
            if (!isDash)    // 대시 중이 아닐 때에만 방향 갱신
            {
                dir = MoveDirection(moveInput);
                moveDirection = (moveInput == Vector2.zero) ?
                                Quaternion.Euler(0f, -45f, 0f) * actor.transform.forward :
                                new(dir.x, 0, dir.y);
                isDash = true;
            }
            QuaterView_Move(moveDirection);
        }
        public void MoveExit()
        {
            isDash = false;
            dir = Vector2.zero;
            moveDirection = Vector3.zero;
        }
    }
}