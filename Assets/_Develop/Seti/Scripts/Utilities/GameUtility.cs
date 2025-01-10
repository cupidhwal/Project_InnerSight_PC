using UnityEngine;

namespace Seti
{
    // 게임 유틸리티
    public static class GameUtility
    {
        // 중력 유틸리티
        public static void Gravity(Transform target, float gravityStrength = 9.81f)
        {
            Vector3 currentVelocity = Vector3.zero;
            Vector3 gravity = new(0, -gravityStrength, 0);
            currentVelocity += gravity * Time.deltaTime;
            target.position += currentVelocity * Time.deltaTime;
        }

        // 마우스로 클릭한 지점의 위치 정보를 반환하는 유틸리티
        public static Vector3 RayToWorldPosition()
        {
            Vector3 hitPosition = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) hitPosition = hit.point;

            return hitPosition;
        }

        // B에 대한 A의 스크린 상 위치를 반환하는 유틸리티
        public static Vector2 ScreenPositionFromA(Camera camera, Transform aTransform, Transform bTransform)
        {
            if (camera == null || aTransform == null || bTransform == null)
            {
                Debug.LogWarning("Camera or Transforms are null.");
                return Vector2.zero;
            }

            // B의 월드 좌표를 스크린 좌표로 변환
            Vector3 screenPosition = camera.WorldToScreenPoint(bTransform.position - aTransform.position);

            // A의 스크린 좌표와 비교 (Z축 제외)
            Vector2 screenPositionRelativeToA = new(
                -screenPosition.x,
                -screenPosition.y
            );
            Debug.Log(screenPositionRelativeToA);
            return screenPositionRelativeToA;
        }
    }
}