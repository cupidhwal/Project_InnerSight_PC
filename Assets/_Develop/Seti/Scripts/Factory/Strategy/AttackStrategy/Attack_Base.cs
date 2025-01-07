using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Attack Behaviour의 Strategy Base
    /// </summary>
    public abstract class Attack_Base : IAttackStrategy
    {
        // 필드
        #region Variables
        // 세팅
        protected Actor actor;
        #endregion

        // 인터페이스
        #region Interface
        // 초기화
        public void Initialize(Actor actor, float power)
        {
            this.actor = actor;
        }

        public Type GetStrategyType() => typeof(IAttackStrategy);

        public abstract void Attack();
        #endregion

        // 메서드
        #region Methods
        #endregion
    }
}