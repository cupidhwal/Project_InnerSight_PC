using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Box Custom Editor
    /// </summary>
    [CustomEditor(typeof(Box_Behaviour))]
    public class Box_Behaviour_Editor : Editor
    {
        private Box_Behaviour behaviourBox;

        public override void OnInspectorGUI()
        {
            behaviourBox = (Box_Behaviour)target;

            EditUtility.SubjectLine(2, "기능 컨테이너");

            // 기본 Inspector 그리기
            DrawDefaultInspector();

            // 리프레시 버튼
            if (GUILayout.Button("Behaviour List 갱신"))
            {
                RefreshBehaviourList();
            }

            // Behaviour 리스트 표시
            DrawBehaviourList();

            EditUtility.DrawLine(2);
        }

        private void RefreshBehaviourList()
        {
            // IBehaviour를 구현한 모든 클래스 탐색
            var allBehaviours = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => typeof(IBehaviour).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            int addedCount = 0;

            foreach (var behaviourType in allBehaviours)
            {
                // 중복 검사
                if (!behaviourBox.behaviours.Any(f => f.GetType() == behaviourType))
                {
                    var newBehaviour = Activator.CreateInstance(behaviourType) as IBehaviour;
                    behaviourBox.behaviours.Add(newBehaviour);
                    addedCount++;
                }
            }

            EditorUtility.SetDirty(behaviourBox); // 변경 사항 저장
            Debug.Log($"Box_Behaviour 갱신: 새 Behaviour가 {addedCount}개 추가되었습니다.");
        }

        private void DrawBehaviourList()
        {
            EditUtility.SubjectLine(Color.gray, 2, "Behaviour List");

            for (int i = 0; i < behaviourBox.behaviours.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(behaviourBox.behaviours[i].GetType().Name);

                if (GUILayout.Button("Remove"))
                {
                    behaviourBox.behaviours.RemoveAt(i);
                    EditorUtility.SetDirty(behaviourBox);
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}