using UnityEngine;

namespace Seti
{
    public abstract class Look_Base : ILookStrategy
    {
        // �ʵ�
        #region Variables
        // ������
        protected float mouseSensitivity;     // ���콺 ����

        // ����
        protected Actor actor;
        protected Rigidbody rb;               // �÷��̾� Rigidbody
        protected Transform headTransform;    // �÷��̾��� �Ӹ� �κ� Transform

        // �Ϲ� �ʵ�
        protected float headXRotation;        // head X�� ȸ����
        protected float headYRotation;        // head Y�� ȸ����
        protected float bodyYRotation;        // body Y�� ȸ����
        #endregion

        // �޼���
        #region Methods
        public void Initialize(Actor actor, float mouseSensitivity)
        {
            this.actor = actor;
            this.mouseSensitivity = mouseSensitivity;

            rb = actor.GetComponent<Rigidbody>();
            headTransform = actor.transform.GetChild(1);
        }

        public abstract void Look(Vector2 readValue);
        #endregion
    }
}