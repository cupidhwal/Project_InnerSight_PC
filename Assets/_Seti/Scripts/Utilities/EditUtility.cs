using UnityEditor;
using UnityEngine;

namespace Seti
{
    public static class EditUtility
    {
        public static void CheckMethod()
        {
            Debug.Log("호출");
        }

        // 주제선 그리기
        public static void SubjectLine(int lineWidth, string text = "")
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(lineWidth));
            GUILayout.Space(5);
        }

        public static void SubjectLine(Color color, int lineWidth, string text = "")
        {
            GUIStyle lineStyle = new();
            lineStyle.normal.background = ColorUtility.CreateColoredTexture(color);

            GUILayout.Space(10);
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
            GUILayout.Box("", lineStyle, GUILayout.ExpandWidth(true), GUILayout.Height(lineWidth));
            GUILayout.Space(5);
        }

        // 구분선 그리기
        public static void DrawLine(int boxheight)
        {
            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(boxheight));
            GUILayout.Space(5);
        }

        public static void DrawLine(Color color, int lineWidth)
        {
            GUIStyle lineStyle = new();
            lineStyle.normal.background = ColorUtility.CreateColoredTexture(color);

            GUILayout.Space(5);
            GUILayout.Box("", lineStyle, GUILayout.ExpandWidth(true), GUILayout.Height(lineWidth));
            GUILayout.Space(5);
        }

        
    }
}