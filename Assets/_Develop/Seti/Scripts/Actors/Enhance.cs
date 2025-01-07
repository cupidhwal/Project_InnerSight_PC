using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Player ����, �ൿ�� ��ȭ�ϴ� ����� �����ϴ� Ŭ����
    /// </summary>
    /// increment�� ������� �Է��� ��
    /// Player ���� Ŭ�����̹Ƿ� Player ������Ʈ�� �����Ѵ�
    [RequireComponent(typeof(Player))]

    [System.Serializable]
    public class Enhance : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        private Player player;
        private Dictionary<Type, IBehaviour> behaviourMap;

// ��� ����
#pragma warning disable 0414
        [Header("Behaviour : Increments (%)")]
        [SerializeField] private float increment_Move = 10f;
#pragma warning restore 0414
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            // �ʱ�ȭ
            player = GetComponent<Player>();
            InitializeBehaviourMap();
        }
        #endregion

        // �޼���
        #region Methods
        // �ൿ ��ȭ
        public void EnhanceBehaviour<T>(float increment) where T : class, IBehaviour
        {
            if (player == null) return;

            // �ൿ �˻� �� ��ȭ
            if (behaviourMap.TryGetValue(typeof(T), out var behaviour))
                (behaviour as T)?.Upgrade(increment);
        }

        // �ൿ ��ȭ - �����ε�, �߾� ���߽�
        public void EnhanceBehaviour<T>() where T : class, IBehaviour
        {
            if (player == null) return;

            // �ൿ �˻�
            if (!behaviourMap.TryGetValue(typeof(T), out var behaviour))
                return;

            // increment_ �ʵ� �˻�
            string incrementFieldName = $"increment_{typeof(T).Name}";
            FieldInfo incrementField = GetType().GetField(incrementFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (incrementField == null || incrementField.FieldType != typeof(float))
            {
                Debug.LogWarning($"'{incrementFieldName}'�� ��ȿ���� ���� Increment �ʵ��Դϴ�.");
                return;
            }

            // ������ ��������
            float increment = (float)incrementField.GetValue(this);

            // �ൿ ��ȭ
            (behaviour as T)?.Upgrade(increment);
        }

        // �ൿ ����
        private void InitializeBehaviourMap()
        {
            behaviourMap = new Dictionary<Type, IBehaviour>();

            foreach (var behaviour in player.Behaviours)
            {
                var type = behaviour.behaviour.GetType();
                behaviourMap[type] = behaviour.behaviour;
            }
        }
        #endregion
    }
}