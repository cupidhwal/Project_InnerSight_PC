using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 카메라 시점을 탑 다운 뷰로로 관리하는 클래스
    /// </summary>
    public class TopDownCamera : MonoBehaviour
    {
        #region Variables
        public Transform target;            //플레이어

        //카메라 속성
        [SerializeField] private float height = 5f;     //플레이어로 부터의 높이
        [SerializeField] private float distance = 10f;  //플레이어로 부터의 거리
        [SerializeField] private float angle = 45f;     //플레이어 백뷰로 부터의 회전 각도
        [SerializeField] private float smoothSpeed = 0.5f;  //카메라 이동 속도

        [SerializeField] private float lookAtHeight = 2f;   //카메라가 바라보는 높이

        private Vector3 refVelocity;                        //카메라 현재 속도
        #endregion

        private void Start()
        {
            HandleCamera();
        }

        private void LateUpdate()
        {
            HandleCamera();
        }

        void HandleCamera()
        {
            if(target == null) return;

            //카메라 위치 설정(플레이어 기준)
            Vector3 worldPosition = (target.forward * -distance) + (Vector3.up * height);
            Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;

            //높이에 따른 위치 보정
            Vector3 flatTargetPosition = target.position;
            flatTargetPosition.y += lookAtHeight;
            //카메라가 이동할 최종 위치
            Vector3 finalPosition = flatTargetPosition + rotateVector;

            //카메라 이동
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);

            //플레이어 바라보기
            transform.LookAt(flatTargetPosition);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            if(target)
            {
                Vector3 lookAtPostion = target.position;
                lookAtPostion.y += lookAtHeight;
                Gizmos.DrawSphere(lookAtPostion, 0.25f);
                Gizmos.DrawLine(transform.position, lookAtPostion);
            }
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}