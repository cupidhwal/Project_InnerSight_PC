using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Move Behaviour�� Strategy Base
    /// </summary>
    public abstract class Move_Base : IMoveStrategy
    {
        // �ʵ�
        #region Variables
        // ����
        protected float speed;
        protected Actor actor;
        protected Rigidbody rb;
        protected Vector2 lastMoveDirection;
        #endregion

        // �������̽�
        #region Interface
        // �ʱ�ȭ
        public void Initialize(Actor actor, float speed)
        {
            this.actor = actor;
            this.speed = speed;
            rb = actor.GetComponent<Rigidbody>();
        }

        public Type GetStrategyType() => typeof(IMoveStrategy);

        // QuaterView - World ���� �̵� ����
        public virtual void Move(Vector2 moveInput)
        {
            if (rb == null) return;

            Vector2 dir = MoveDirection(moveInput);
            Vector3 moveDirection = new(dir.x, 0, dir.y);

            Move(moveDirection);
        }
        protected void Move(Vector3 moveDirection)
        {
            Vector3 move = speed * Time.fixedDeltaTime * moveDirection.normalized;
            Vector3 QuaterView = Quaternion.Euler(0f, 45f, 0f) * move;
            rb.MovePosition(actor.transform.position + QuaterView);

            // �̵��� �߻��� ���� ȸ��
            if (moveDirection != Vector3.zero)
            {
                // ���� �������� ȸ��
                Quaternion targetRotation = Quaternion.LookRotation(QuaterView, Vector3.up);
                rb.MoveRotation(Quaternion.Slerp(actor.transform.rotation, targetRotation, 10f * Time.fixedDeltaTime));
            }
        }

        // Local ���� �̵� ����
        /*public void Move(Vector2 moveInput)
        {
            if (rb == null) return; 

            Vector2 dir = MoveDirection(moveInput);
            Vector3 moveDirection = new(dir.x, 0, dir.y);

            Vector3 forward = actor.transform.forward * moveDirection.z;
            Vector3 right = actor.transform.right * moveDirection.x;

            Vector3 move = speed * Time.fixedDeltaTime * (forward + right).normalized;
            rb.MovePosition(actor.transform.position + move);
        }*/

        // ������ ����
        public void GetOverCurb(Collision collision)
        {
            float height = 0;
            if (collision.transform.TryGetComponent<BoxCollider>(out var curb))
            {
                height = curb.size.y * collision.transform.localScale.y / 2 +
                         curb.center.y +
                         collision.transform.position.y -
                         actor.transform.position.y;
            }
            else if (collision.transform.TryGetComponent<MeshCollider>(out var meshCurb))
            {
                ContactPoint contact = collision.contacts[0];
                Bounds bounds = meshCurb.bounds;
                float sqrContactDis = (contact.point - bounds.center).sqrMagnitude;
                float sqrCenterDis = new Vector3(bounds.size.x / 2, bounds.size.y / 2, bounds.size.z / 2).sqrMagnitude;

                if (sqrContactDis > sqrCenterDis / 2)
                {
                    height = bounds.size.y * collision.transform.localScale.y / 2 +
                             bounds.center.y +
                             collision.transform.position.y -
                             actor.transform.position.y;
                }
            }

            if (height > 0f && height < 0.5f)
                rb.MovePosition(actor.transform.position + new Vector3(0, height, 0));
        }
        #endregion

        // �޼���
        #region Methods
        // ���� ���� ���� ����
        protected Vector2 MoveDirection(Vector2 moveInput)
        {
            State_Common state = actor.ActorState as State_Common;
            if (state.IsGrounded)
                lastMoveDirection = moveInput;
            return lastMoveDirection;
        }
        #endregion
    }
}