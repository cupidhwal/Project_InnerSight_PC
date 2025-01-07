using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Strategy ����
    /// </summary>
    [CreateAssetMenu(fileName = "Box_Strategy", menuName = "Database/Box_Strategy")]
    public class Box_Strategy : ScriptableObject
    {
        // ���� ����Ʈ (��� ���� ����)
        [HideInInspector]
        [SerializeReference]
        public List<ILookStrategy> lookStrategies;

        [HideInInspector]
        [SerializeReference]
        public List<IMoveStrategy> moveStrategies;

        [HideInInspector]
        [SerializeReference]
        public List<IJumpStrategy> jumpStrategies;

        // ���� ���� �˻�
        public List<T> GetStrategies<T>() where T : class, IStrategy
        {
            List<T> list = new();

            // T Ÿ�Կ� �´� ����Ʈ ����
            var targetList = GetStrategyList<T>();
            if (targetList == null)
            {
                Debug.LogWarning($"�ش� ���� ����Ʈ�� �����ϴ�: {typeof(T)}");
                return list;
            }

            // ����Ʈ���� Ÿ�� �˻� �� �߰�
            foreach (var strategy in targetList)
            {
                if (strategy is T targetStrategy)
                {
                    list.Add(targetStrategy);
                }
            }

            if (list.Count == 0)
                Debug.LogWarning($"�ش� ������ �����ϴ�: {typeof(T)}");

            return list;
        }

        // Ư�� Ÿ���� ���� �˻�
        public T GetStrategy<T>() where T : class, IStrategy
        {
            // T Ÿ�Կ� �´� ����Ʈ ����
            var targetList = GetStrategyList<T>();
            if (targetList == null)
            {
                Debug.LogWarning($"�ش� ���� ����Ʈ�� �����ϴ�: {typeof(T)}");
                return null;
            }

            // ù ��° ��ġ�ϴ� ���� ��ȯ
            foreach (var strategy in targetList)
            {
                if (strategy is T targetStrategy)
                {
                    return targetStrategy;
                }
            }

            Debug.LogWarning($"�ش� ������ �����ϴ�: {typeof(T)}");
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

            // ���ο� ������ �߰��Ǹ� ���� �������� �߰�
            //else if (typeof(T) == typeof(IAttackStrategy))
            //    return attackStrategies as IEnumerable<IStrategy>;

            return null; // ��ġ�ϴ� ����Ʈ�� ���� ���
        }
    }
}