using System;
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
        BT
    }

    [Serializable]
    public class BehaviourStrategyMapping
    {
        [SerializeReference]
        public IBehaviour behaviour;
        [SerializeReference]
        public List<Strategy> strategies;
    }

    /// <summary>
    /// Actor의 Blueprint
    /// </summary>
    [CreateAssetMenu(fileName = "New Actor", menuName = "Blueprint/Actor")]
    public class Blueprint_Actor : ScriptableObject
    {
        // 필드
        #region Variables
        [SerializeField]
        private string actorName;           // 액터 이름
        public GameObject actorPrefab;      // 액터 오브젝트
        public ControlType controlType;     // Actor 타입(enum)
        public Box_Strategy strategyBox;    // Box_Strategy 참조
        public Box_Behaviour behaviourBox;  // Box_Behaviour 참조

        [HideInInspector]
        [SerializeReference]
        public List<BehaviourStrategyMapping> behaviourStrategies = new();

        public string ActorName => actorName;
        #endregion

        // 특정 행동에 대한 전략 가져오기
        public List<Strategy> GetStrategies(IBehaviour behaviour)
        {
            var mapping = behaviourStrategies.FirstOrDefault(mapping => mapping.behaviour == behaviour);

            if (behaviour is IHasStrategy behaviourWithStrategy)
            {
                // 활성화된 전략만 새로 생성
                return mapping.strategies
                    .Where(s => s.isActive)
                    .Select(s => new Strategy
                    {
                        strategy = s.strategy,
                        isActive = s.isActive
                    }).ToList();
            }
            return null;
        }

        // 특정 전략의 활성 상태 업데이트
        public void UpdateStrategy(IBehaviour behaviour, IStrategy strategy, bool isActive)
        {
            var mapping = behaviourStrategies.FirstOrDefault(m => m.behaviour == behaviour);
            if (mapping != null)
            {
                var target = mapping.strategies.FirstOrDefault(s => s.strategy == strategy);
                if (target != null)
                {
                    target.isActive = isActive;
#if UNITY_EDITOR
                    EditorUtility.SetDirty(this); // 변경 사항 저장
#endif
                }
            }
        }
    }
}