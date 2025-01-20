using UnityEngine;

namespace VFXTools
{
    public class RotateAround : MonoBehaviour
    {
        //public Transform target;            
        //public float rotationSpeed = 30.0f; 
        //public float rotationRadius = 2.0f; 
        //private Vector3 initialPosition;

        //void Start()
        //{
        //    initialPosition = transform.position;
        //    target = GameObject.FindWithTag("Player").transform;
        //}

        //void Update()
        //{
        //    if (target != null)
        //    {
        //        float angle = Time.time * rotationSpeed;
        //        float x = initialPosition.x + rotationRadius * Mathf.Cos(angle);
        //        float z = initialPosition.z + rotationRadius * Mathf.Sin(angle);

        //        transform.position = new Vector3(x, transform.position.y, z);

        //        transform.LookAt(target, Vector3.up);
        //    }
        //}

        // 중심이 되는 오브젝트
        public Transform centerPoint;

        // 회전 속도 (각도/초)
        public float rotationSpeed = 50f;

        // 궤도 반지름
        public float orbitRadius = 5f;

        // 초기 각도
        private float currentAngle;

        void Start()
        {
            centerPoint = GameObject.FindWithTag("Player").transform;

            // 초기 위치를 설정합니다.
            Vector3 direction = (transform.position - centerPoint.position).normalized;
            transform.position = centerPoint.position + direction * orbitRadius;
        }

        void Update()
        {
            if (centerPoint == null)
                return;

            // 각도 업데이트 (시간에 따라)
            currentAngle += rotationSpeed * Time.deltaTime;

            // 라디안으로 변환
            float angleInRadians = currentAngle * Mathf.Deg2Rad;

            // 새로운 위치 계산
            float x = Mathf.Cos(angleInRadians) * orbitRadius;
            float z = Mathf.Sin(angleInRadians) * orbitRadius;

            // 오브젝트 위치 업데이트
            transform.position = new Vector3(centerPoint.position.x + x, transform.position.y, centerPoint.position.z + z);

            // 중심을 향해 회전하도록 설정하려면 아래 코드 활성화
            transform.LookAt(centerPoint);
        }
    }
}
