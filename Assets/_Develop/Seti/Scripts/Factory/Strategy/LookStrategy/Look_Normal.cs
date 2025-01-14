using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 마우스를 통한 기본 시야 제어
    /// </summary>
    public class Look_Normal : Look_Base
    {
        public override void Look(Vector2 readValue)
        {
            if (actor is not Player player || player.View != Player.ViewType.Follow_Person) return;

            // 각 축의 Delta 값
            headXRotation -= readValue.y * mouseSensitivity;

            // 각 축의 한계 회전각
            headXRotation = Mathf.Clamp(headXRotation, -50f, 50f);
            headYRotation = Mathf.Clamp(headYRotation, -80f, 80f);

            if (readValue != Vector2.zero)
                bodyYRotation = readValue.x * mouseSensitivity;
            else
                bodyYRotation = 0;

            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, bodyYRotation, 0f));
            headTransform.localRotation = Quaternion.Euler(headXRotation, headYRotation, 0f);
        }
    }
}