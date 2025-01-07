using System;
using System.Collections.Generic;

namespace Seti
{
    /// <summary>
    /// Strategy�� ���� Behaviour�� �Ǻ�
    /// </summary>
    public interface IHasStrategy
    {
        public void SetStrategies(IEnumerable<Strategy> strategies);
        public void ChangeStrategy(Type strategyType);
        public Type GetStrategyType();
    }
}