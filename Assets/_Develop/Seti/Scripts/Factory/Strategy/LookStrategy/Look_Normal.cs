using UnityEngine;

namespace Seti
{
    public class Look_Normal : Look_Base
    {
        public override void Look(Vector2 readValue)
        {
            // �� ���� Delta ��
            headXRotation -= readValue.y * mouseSensitivity;

            // �� ���� �Ѱ� ȸ����
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