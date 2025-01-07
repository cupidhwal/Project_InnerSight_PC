using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Seti
{
    public enum ControlType
    {
        Input,
        FSM,
        Stuff
    }

    /// <summary>
    /// Actor�� Blueprint
    /// </summary>
    [CreateAssetMenu(fileName = "New Actor", menuName = "Blueprint/Actor")]
    public class Blueprint_Actor : ScriptableObject
    {
        [SerializeField]
        private string actorName; // ���� �̸�
        public GameObject actorPrefab; // ���� ������Ʈ
        public ControlType controlType; // Actor Ÿ��(enum)
        public Box_Strategy strategyBox; // Box_Strategy ����
        public Box_Behaviour behaviourBox; // Box_Behaviour ����

        [HideInInspector]
        [SerializeReference]
        public List<BehaviourStrategyMapping> behaviourStrategies = new();

        public string ActorName => actorName;

        // Ư�� �ൿ�� ���� ���� ��������
        public List<Strategy> GetStrategiesForBehaviour(IBehaviour behaviour)
        {
            return behaviourStrategies
                .FirstOrDefault(mapping => mapping.behaviour == behaviour)?
                .strategies ?? new List<Strategy>();
        }

        // Ư�� ������ Ȱ�� ���� ������Ʈ
        public void UpdateStrategy(IBehaviour behaviour, IStrategy strategy, bool isActive)
        {
            var mapping = behaviourStrategies.FirstOrDefault(m => m.behaviour == behaviour);
            if (mapping != null)
            {
                var target = mapping.strategies.FirstOrDefault(s => s.strategy == strategy);
                if (target != null)
                {
                    target.isActive = isActive;
                    EditorUtility.SetDirty(this); // ���� ���� ����
                }
            }
        }
    }

    [System.Serializable]
    public class BehaviourStrategyMapping
    {
        [SerializeReference]
        public IBehaviour behaviour;
        [SerializeReference]
        public List<Strategy> strategies;
    }
}