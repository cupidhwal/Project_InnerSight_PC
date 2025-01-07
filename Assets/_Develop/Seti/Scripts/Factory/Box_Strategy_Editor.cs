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

    // ���� �˻� ����
    [CustomEditor(typeof(Box_Strategy))]
    public class Box_Strategy_Editor : Editor
    {
        private Box_Strategy strategyBox;
        private readonly GUIStyle lineStyle = new();                    // �÷� ���м� �߰�

        public override void OnInspectorGUI()
        {
            // ���м� ���� ����
            lineStyle.normal.background = ColorUtility.CreateColoredTexture(Color.gray);

            strategyBox = (Box_Strategy)target;

            EditUtility.SubjectLine(2, "���� �����̳�");

            // �⺻ Inspector �׸���
            DrawDefaultInspector();

            // ����ġ ���� ��ư
            if (GUILayout.Button("Strategy List ����"))
            {
                RefreshAllStrategies();
            }

            GUILayout.Space(10);

            // Strategy ����Ʈ ǥ��
            DrawStrategyList();

            EditUtility.DrawLine(2);
        }

        private void RefreshAllStrategies()
        {
            // ��� ���� ����Ʈ�� ����
            RefreshStrategyList(strategyBox.lookStrategies);
            RefreshStrategyList(strategyBox.moveStrategies);
            RefreshStrategyList(strategyBox.jumpStrategies);

            // ���ο� ���� ����Ʈ�� �ִٸ� ���⿡ �߰�
            // RefreshStrategyList(strategyBox.attackStrategies);
            // RefreshStrategyList(strategyBox.defenseStrategies);

            // ���� ���� ����
            EditorUtility.SetDirty(strategyBox);
            //Debug.Log("��� ���� ����Ʈ�� ���ŵǾ����ϴ�.");
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

            Debug.Log($"{typeof(T).Name} ���� ����: �� {typeof(T).Name}�� {addedCount}�� �߰��Ǿ����ϴ�.");
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

            // �߰� ���� ����Ʈ ����
            // EditorGUILayout.LabelField("Attack Strategies", EditorStyles.boldLabel);
            // DrawStrategySubList(strategyBox.attackStrategies);
        }

        private void DrawStrategySubList<T>(List<T> strategyList) where T : class, IStrategy
        {
            // Null ��� ����
            strategyList.RemoveAll(item => item == null);

            // "Normal" ������ �׻� ù ��°�� ������ ����
            strategyList = strategyList
                .OrderBy(s => !s.GetType().Name.EndsWith("_Normal"))    // "_Normal"�� ������ ������ ���� ���� ��ġ
                .ThenBy(s => s.GetType().Name)  // �������� ���ĺ� ������ ����
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
                        break; // ����Ʈ�� �����Ǿ����Ƿ� ���� �ߴ�
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