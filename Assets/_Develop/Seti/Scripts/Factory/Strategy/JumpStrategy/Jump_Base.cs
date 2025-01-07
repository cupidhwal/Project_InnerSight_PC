using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Jump Behaviour의 Strategy Base
    /// </summary>
    public abstract class Jump_Base : IJumpStrategy
    {
        // 필드
        #region Variables
        // 세팅
        protected float jumpForce;
        protected Actor actor;
        protected Rigidbody rb;     // 플레이어 Rigidbody
        #endregion

        // 메서드
        #region Methods
        public void Initialize(Actor actor, float jumpForce)
        {
            this.jumpForce = jumpForce;
            this.actor = actor;
            rb = actor.GetComponent<Rigidbody>();
        }

        public Type GetStrategyType() => typeof(IJumpStrategy);

        // 플레이어의 기본 점프 메서드
        public abstract void Jump();
        #endregion
    }
}