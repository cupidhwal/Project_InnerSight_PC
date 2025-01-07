using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Seti
{
    [CustomEditor(typeof(Blueprint_Actor))]
    public class Blueprint_Actor_Editor : Editor
    {
        // 필드
        #region Variables
        private Blueprint_Actor blueprint;
        private string[] behaviourTypeNames;
        private List<IBehaviour> availableBehaviours;
        #endregion

        // 메서드
        #region Methods
        public override void OnInspectorGUI()
        {
            //if (Application.isPlaying) return;

            blueprint = target as Blueprint_Actor;

            GUILayout.Space(10);
            EditorGUILayout.LabelField($"{blueprint.ActorName} 설계도", EditorStyles.boldLabel);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(5);

            DrawDefaultInspector();

            if (blueprint.behaviourBox == null)
            {
                EditorGUILayout.HelpBox("BehaviourBox를 참조하세요!", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("추가할 수 있는 Behaviours", EditorStyles.boldLabel);

            // BehaviourBox에서 기능 목록 가져오기
            UpdateAvailableBehaviours();

            // 드롭다운 메뉴로 기능 선택
            if (behaviourTypeNames != null && behaviourTypeNames.Length > 0)
            {
                int selectedIndex = EditorGUILayout.Popup(-1, behaviourTypeNames, GUILayout.Width(300));
                if (selectedIndex >= 0)
                {
                    AddBehaviourToBlueprint(availableBehaviours[selectedIndex]);
                }
            }

            // 현재 Blueprint의 기능 리스트 표시
            DrawBehavioursList();
        }

        private void UpdateAvailableBehaviours()
        {
            if (blueprint.behaviourBox == null || blueprint.behaviourBox.behaviours == null)
                return;

            var existingBehaviourTypes = blueprint.behaviourStrategies
                .Select(mapping => mapping.behaviour.GetType())
                .ToHashSet();

            availableBehaviours = blueprint.behaviourBox.behaviours
                .Where(behaviour => !existingBehaviourTypes.Contains(behaviour.GetType()))
                .ToList();

            behaviourTypeNames = availableBehaviours
                .Select(behaviour => behaviour.GetType().Name)
                .ToArray();
        }

        private void AddBehaviourToBlueprint(IBehaviour behaviour)
        {
            if (!blueprint.behaviourStrategies.Any(mapping => mapping.behaviour == behaviour))
            {
                var strategies = new List<Strategy>();
                if (behaviour is IHasStrategy hasStrategy)
                {
                    var strategyType = hasStrategy.GetStrategyType();
                    if (strategyType != null)
                    {
                        var method = typeof(Box_Strategy).GetMethod(nameof(Box_Strategy.GetStrategies));
                        if (method != null)
                        {
                            var genericMethod = method.MakeGenericMethod(strategyType);
                            if (genericMethod.Invoke(blueprint.strategyBox, null) is IEnumerable<IStrategy> allStrategies)
                            {
                                strategies = allStrategies
                                    .Select(strategy => new Strategy
                                    {
                                        strategy = strategy,
                                        isActive = true
                                    })
                                    .ToList();
                            }
                        }
                    }
                }

                blueprint.behaviourStrategies.Add(new BehaviourStrategyMapping
                {
                    behaviour = behaviour,
                    strategies = strategies
                });

                EditorUtility.SetDirty(blueprint);
            }
            else
            {
                Debug.LogWarning($"{behaviour.GetType().Name}은 이미 추가되었습니다!");
            }
        }

        private void DrawBehavioursList()
        {
            EditUtility.SubjectLine(Color.gray, 2, "이 Actor의 Behaviours");

            foreach (var mapping in blueprint.behaviourStrategies)
            {
                var behaviour = mapping.behaviour;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(behaviour.GetType().Name, EditorStyles.boldLabel, GUILayout.Width(200));

                if (GUILayout.Button("제거", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    blueprint.behaviourStrategies.Remove(mapping);
                    EditorUtility.SetDirty(blueprint);
                    break; // 리스트 변경으로 인해 foreach 종료
                }
                EditorGUILayout.EndHorizontal();

                DrawStrategiesList(mapping);

                EditUtility.DrawLine(Color.gray, 2);
            }
        }

        private void DrawStrategiesList(BehaviourStrategyMapping mapping)
        {
            var strategies = mapping.strategies;

            if (strategies.Count == 0) return;

            strategies = strategies
                .OrderBy(s => s.strategy.GetType().Name != $"{mapping.behaviour.GetType().Name}_Normal")
                .ThenBy(s => s.strategy.GetType().Name)
                .ToList();

            EditUtility.DrawLine(1);

            EditorGUILayout.LabelField($"{mapping.behaviour.GetType().Name} Strategies", EditorStyles.boldLabel);

            foreach (var strategyData in strategies)
            {
                EditorGUILayout.BeginHorizontal();

                GUI.color = strategyData.isActive ? Color.white : new Color(1f, 1f, 1f, 0.5f);
                GUI.backgroundColor = strategyData.isActive ? Color.green : Color.gray;

                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField(strategyData.strategy.GetType().Name, GUILayout.Width(193));
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button(strategyData.isActive ? "제거" : "추가", GUILayout.Width(100), GUILayout.Height(26)))
                {
                    blueprint.UpdateStrategy(mapping.behaviour, strategyData.strategy, !strategyData.isActive);
                }

                EditorGUILayout.EndHorizontal();
                GUI.color = Color.white;
                GUI.backgroundColor = Color.white;
            }
        }
        #endregion
    }
}