using System;
using System.Collections.Generic;

namespace Seti
{
    /// <summary>
    /// Strategy를 갖는 Behaviour의 판별
    /// </summary>
    public interface IHasStrategy
    {
        public void SetStrategies(IEnumerable<Strategy> strategies);
        public void ChangeStrategy(Type strategyType);
        public Type GetStrategyType();
    }
}