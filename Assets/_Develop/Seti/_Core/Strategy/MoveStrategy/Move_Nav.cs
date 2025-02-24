using UnityEngine;

namespace Seti
{
    public class Move_Nav : Move_Base
    {
        protected override void QuaterView_Move(Vector2 moveInput)
        {
            Vector3 moveDirection = new(dir.x, 0, dir.y);
            Vector3 QuaterView = Quaternion.Euler(0f, 45f, 0f) * moveDirection;
            QuaterView_Rot(QuaterView);
        }
    }
}