using UnityEngine;

namespace Noah
{
    public class RayManager : Singleton<RayManager>
    {
        private Transform player;
        public LayerMask groundLayerMask;
        private Camera mainCamera;

        private void Start()
        {
            Init();
        }

        void Init()
        {
            mainCamera = Camera.main;
            player = transform;
        }

        public Vector3 RayToScreen()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, groundLayerMask))
            {
                Debug.Log(hit.collider.name);

                return hit.point;
            }

            return hit.point;
        }

        public Quaternion UpdateSkillRangeRotation()
        {
            // 마우스 위치 가져오기
            Vector3 mousePosition = Input.mousePosition;
            Quaternion targetRotation = Quaternion.identity;

            // 마우스 위치를 월드 좌표로 변환
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 방향 계산 (오브젝트 위치 -> 마우스 위치)
                Vector3 direction = hit.point - transform.position;

                // 방향에 따라 회전 적용
                targetRotation = Quaternion.LookRotation(direction);

                return targetRotation;
            }

            return targetRotation;
        }
    }
}