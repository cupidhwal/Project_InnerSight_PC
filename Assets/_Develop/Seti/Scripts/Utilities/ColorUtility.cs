using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Seti
{
    // Image 컴포넌트의 Color/Alpha 값을 제어하는 유틸리티
    public class ColorUtility
    {
        // 텍스트의 알파값을 설정하는 메서드
        public static void SetAlpha(TextMeshProUGUI text, float alpha)
        {
            if (text == null) return;

            // 현재 이미지의 색상을 가져옴
            Color color = text.color;

            // 알파값을 변경
            color.a = Mathf.Clamp01(alpha); // 0.0f ~ 1.0f 사이로 제한

            // 변경된 색상을 다시 이미지에 할당
            text.color = color;
        }

        // 이미지의 알파값을 설정하는 메서드
        public static void SetAlpha(Image image, float alpha)
        {
            if (image == null) return;

            // 현재 이미지의 색상을 가져옴
            Color color = image.color;

            // 알파값을 변경
            color.a = Mathf.Clamp01(alpha); // 0.0f ~ 1.0f 사이로 제한

            // 변경된 색상을 다시 이미지에 할당
            image.color = color;
        }

        // 이미지의 전체 색상을 변경하는 메서드
        public static void SetColor(Image image, Color color)
        {
            if (image == null) return;

            // 이미지에 새로운 색상 할당
            image.color = color;
        }

        // 구분선의 색상을 설정하는 함수
        public static Texture2D CreateColoredTexture(Color color)
        {
            Texture2D texture = new(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        /*// 입력한 점들을 UI라인렌더러로 연결하는 유틸리티
        public static UILineRenderer DrawUILine(GameObject targetObject, List<Vector2> points, float lineWidth, Color color, float alpha)
        {
            // UILineRenderer 컴포넌트를 추가하거나 기존의 것을 사용
            if (!targetObject.TryGetComponent<UILineRenderer>(out var uiLineRenderer))
            {
                uiLineRenderer = targetObject.AddComponent<UILineRenderer>();
            }

            // UILineRenderer의 기본 설정
            uiLineRenderer.Points = points.ToArray();
            uiLineRenderer.LineThickness = lineWidth;

            // 라인의 색상 및 알파값 설정
            color.a = alpha;
            uiLineRenderer.color = color;

            return uiLineRenderer;
        }

        // Vector2, 입력한 점들을 라인렌더러로 연결하는 유틸리티
        public static LineRenderer DrawLine(GameObject targetObject, List<Vector2> points, float lineWidth, Color color, float alpha)
        {
            // LineRenderer 컴포넌트를 추가하거나 기존의 것을 사용
            if (!targetObject.TryGetComponent<LineRenderer>(out var lineRenderer))
            {
                lineRenderer = targetObject.AddComponent<LineRenderer>();
            }
            List<Vector3> points3D = points.ConvertAll(points => (Vector3)points);

            // LineRenderer의 기본 설정
            lineRenderer.positionCount = points3D.Count;
            lineRenderer.SetPositions(points3D.ToArray());
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;

            // 라인의 색상 및 알파값 설정
            color.a = alpha;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            // 머티리얼 설정 (기본적으로 Unity에서 제공하는 LineRenderer Material 사용)
            if (lineRenderer.material == null)
            {
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            }

            return lineRenderer;
        }

        // Vector3, 입력한 점들을 라인렌더러로 연결하는 유틸리티
        public static LineRenderer DrawLine(GameObject targetObject, List<Vector3> points, float lineWidth, Color color, float alpha)
        {
            // LineRenderer 컴포넌트를 추가하거나 기존의 것을 사용
            LineRenderer lineRenderer = targetObject.GetComponent<LineRenderer>() ?? targetObject.AddComponent<LineRenderer>();

            // LineRenderer의 기본 설정
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;

            // 라인의 색상 및 알파값 설정
            color.a = alpha;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            // 머티리얼 설정 (기본적으로 Unity에서 제공하는 LineRenderer Material 사용)
            if (lineRenderer.material == null)
            {
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            }

            return lineRenderer;
        }*/
    }
}