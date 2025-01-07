using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Attack Behaviour�� Strategy Base
    /// </summary>
    public abstract class Attack_Base : IAttackStrategy
    {
        // �ʵ�
        #region Variables
        // ����
        protected Actor actor;
        #endregion

        // �������̽�
        #region Interface
        // �ʱ�ȭ
        public void Initialize(Actor actor, float power)
        {
            this.actor = actor;
        }

        public Type GetStrategyType() => typeof(IAttackStrategy);

        public abstract void Attack();
        #endregion

        // �޼���
        #region Methods
        #endregion
    }
}