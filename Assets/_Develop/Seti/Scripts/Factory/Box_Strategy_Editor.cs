using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Strategy Box Custom Editor
    /// </summary>

    // 수동 검색 버전
    [CustomEditor(typeof(Box_Strategy))]
    public class Box_Strategy_Editor : Editor
    {
        private Box_Strategy strategyBox;
        private readonly GUIStyle lineStyle = new();                    // 컬러 구분선 추가

        public override void OnInspectorGUI()
        {
            // 구분선 색상 설정
            lineStyle.normal.background = ColorUtility.CreateColoredTexture(Color.gray);

            strategyBox = (Box_Strategy)target;

            EditUtility.SubjectLine(2, "전략 컨테이너");

            // 기본 Inspector 그리기
            DrawDefaultInspector();

            // 원터치 갱신 버튼
            if (GUILayout.Button("Strategy List 갱신"))
            {
                RefreshAllStrategies();
            }

            GUILayout.Space(10);

            // Strategy 리스트 표시
            DrawStrategyList();

            EditUtility.DrawLine(2);
        }

        private void RefreshAllStrategies()
        {
            // 모든 전략 리스트를 갱신
            RefreshStrategyList(strategyBox.lookStrategies);
            RefreshStrategyList(strategyBox.moveStrategies);
            RefreshStrategyList(strategyBox.jumpStrategies);

            // 새로운 전략 리스트가 있다면 여기에 추가
            // RefreshStrategyList(strategyBox.attackStrategies);
            // RefreshStrategyList(strategyBox.defenseStrategies);

            // 변경 사항 저장
            EditorUtility.SetDirty(strategyBox);
            //Debug.Log("모든 전략 리스트가 갱신되었습니다.");
        }

        private void RefreshStrategyList<T>(List<T> strategyList) where T : class, IStrategy
        {
            var allStrategies = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            int addedCount = 0;

            foreach (var strategyType in allStrategies)
            {
                if (!strategyList.Any(s => s != null && s.GetType() == strategyType))
                {
                    var newStrategy = Activator.CreateInstance(strategyType) as T;
                    if (newStrategy != null)
                    {
                        strategyList.Add(newStrategy);
                        addedCount++;
                    }
                }
            }

            Debug.Log($"{typeof(T).Name} 전략 갱신: 새 {typeof(T).Name}가 {addedCount}개 추가되었습니다.");
        }

        private void DrawStrategyList()
        {
            EditUtility.SubjectLine(Color.gray, 2, "Strategy List");

            // Look Strategies
            EditorGUILayout.LabelField("Look Strategies", EditorStyles.boldLabel);
            DrawStrategySubList(strategyBox.lookStrategies);
            EditUtility.DrawLine(Color.gray, 1);

            // Move Strategies
            EditorGUILayout.LabelField("Move Strategies", EditorStyles.boldLabel);
            DrawStrategySubList(strategyBox.moveStrategies);
            EditUtility.DrawLine(Color.gray, 1);

            // Jump Strategies
            EditorGUILayout.LabelField("Jump Strategies", EditorStyles.boldLabel);
            DrawStrategySubList(strategyBox.jumpStrategies);

            // 추가 전략 리스트 예시
            // EditorGUILayout.LabelField("Attack Strategies", EditorStyles.boldLabel);
            // DrawStrategySubList(strategyBox.attackStrategies);
        }

        private void DrawStrategySubList<T>(List<T> strategyList) where T : class, IStrategy
        {
            // Null 요소 제거
            strategyList.RemoveAll(item => item == null);

            // "Normal" 전략이 항상 첫 번째로 오도록 정렬
            strategyList = strategyList
                .OrderBy(s => !s.GetType().Name.EndsWith("_Normal"))    // "_Normal"로 끝나는 전략을 가장 먼저 배치
                .ThenBy(s => s.GetType().Name)  // 나머지는 알파벳 순으로 정렬
                .ToList();

            for (int i = 0; i < strategyList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if (strategyList[i] != null)
                {
                    EditorGUILayout.LabelField(strategyList[i].GetType().Name);

                    if (GUILayout.Button("Remove"))
                    {
                        strategyList.RemoveAt(i);
                        EditorUtility.SetDirty(strategyBox);
                        break; // 리스트가 수정되었으므로 루프 중단
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("<NULL>");
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}