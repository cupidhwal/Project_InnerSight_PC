using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Strategy 상자
    /// </summary>
    [CreateAssetMenu(fileName = "Box_Strategy", menuName = "Database/Box_Strategy")]
    public class Box_Strategy : ScriptableObject
    {
        // 전략 리스트 (모든 전략 저장)
        [HideInInspector]
        [SerializeReference]
        public List<ILookStrategy> lookStrategies;

        [HideInInspector]
        [SerializeReference]
        public List<IMoveStrategy> moveStrategies;

        [HideInInspector]
        [SerializeReference]
        public List<IJumpStrategy> jumpStrategies;

        // 전략 범주 검색
        public List<T> GetStrategies<T>() where T : class, IStrategy
        {
            List<T> list = new();

            // T 타입에 맞는 리스트 참조
            var targetList = GetStrategyList<T>();
            if (targetList == null)
            {
                Debug.LogWarning($"해당 전략 리스트가 없습니다: {typeof(T)}");
                return list;
            }

            // 리스트에서 타입 검사 후 추가
            foreach (var strategy in targetList)
            {
                if (strategy is T targetStrategy)
                {
                    list.Add(targetStrategy);
                }
            }

            if (list.Count == 0)
                Debug.LogWarning($"해당 전략이 없습니다: {typeof(T)}");

            return list;
        }

        // 특정 타입의 전략 검색
        public T GetStrategy<T>() where T : class, IStrategy
        {
            // T 타입에 맞는 리스트 참조
            var targetList = GetStrategyList<T>();
            if (targetList == null)
            {
                Debug.LogWarning($"해당 전략 리스트가 없습니다: {typeof(T)}");
                return null;
            }

            // 첫 번째 일치하는 전략 반환
            foreach (var strategy in targetList)
            {
                if (strategy is T targetStrategy)
                {
                    return targetStrategy;
                }
            }

            Debug.LogWarning($"해당 전략이 없습니다: {typeof(T)}");
            return null;
        }

        public IEnumerable<IStrategy> GetStrategyList<T>() where T : class, IStrategy
        {
            if (typeof(T) == typeof(ILookStrategy))
                return lookStrategies as IEnumerable<IStrategy>;
            else if (typeof(T) == typeof(IMoveStrategy))
                return moveStrategies as IEnumerable<IStrategy>;
            else if (typeof(T) == typeof(IJumpStrategy))
                return jumpStrategies as IEnumerable<IStrategy>;

            // 새로운 전략이 추가되면 같은 형식으로 추가
            //else if (typeof(T) == typeof(IAttackStrategy))
            //    return attackStrategies as IEnumerable<IStrategy>;

            return null; // 일치하는 리스트가 없을 경우
        }
    }
}