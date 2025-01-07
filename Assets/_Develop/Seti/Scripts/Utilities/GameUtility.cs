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
    }
}