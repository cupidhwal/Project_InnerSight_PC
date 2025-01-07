using UnityEngine;

namespace Seti
{
    public abstract class Look_Base : ILookStrategy
    {
        // 필드
        #region Variables
        // 조정값
        protected float mouseSensitivity;     // 마우스 감도

        // 세팅
        protected Actor actor;
        protected Rigidbody rb;               // 플레이어 Rigidbody
        protected Transform headTransform;    // 플레이어의 머리 부분 Transform

        // 일반 필드
        protected float headXRotation;        // head X축 회전값
        protected float headYRotation;        // head Y축 회전값
        protected float bodyYRotation;        // body Y축 회전값
        #endregion

        // 메서드
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