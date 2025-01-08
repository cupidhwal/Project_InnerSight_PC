using System;

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
        protected State_Common state;
        #endregion

        // �������̽�
        #region Interface
        // �ʱ�ȭ
        public void Initialize(Actor actor, float power = 10f)
        {
            this.actor = actor;
            state = actor.ActorState as State_Common;
        }
        public Type GetStrategyType() => typeof(IAttackStrategy);
        public virtual void Attack() => state.IsAttack = true;
        public virtual void AttackExit() => state.IsAttack = false;
        #endregion

        // �޼���
        #region Methods
        #endregion
    }
}