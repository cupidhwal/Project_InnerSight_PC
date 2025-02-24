using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Strategy Pattern - Root
    /// </summary>
    public interface IStrategy
    {
        Type GetStrategyType();
    }

    [System.Serializable]
    public class Strategy
    {
        [SerializeReference]
        public IStrategy strategy;  // 실제 전략 객체
        public bool isActive;       // 전략 활성화 상태

        public Strategy() { }
    }
}