using UnityEngine;

namespace Noah
{
    public class Item_Gold : MonoBehaviour
    {
        public int chargeGold = 10;
        private Transform effect_obj;
        public Transform player;        // 플레이어 Transform
        private Vector3 startPos;

        public float attractionRange = 5f;  // 플레이어와의 감지 거리
        public float attractionSpeed = 2f; // 플레이어로 이동하는 속도

        private float amplitude = 0.2f;  // 오브젝트가 이동할 최대 거리
        private float frequency = 2f;    // 진동의 속도

        private bool isTracking = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            startPos = transform.position;
            player = FindAnyObjectByType<RayManager>().transform;

        }

        // Update is called once per frame
        void Update()
        {
            if (!isTracking)
            {
                MoveObject();
            }

            TrackingPlayer();
        }

        void MoveObject()
        {
            // 시간에 따라 y 위치를 sin 함수로 변경
            float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

            // 새로운 위치로 이동
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }

        void TrackingPlayer()
        {
            isTracking = true;

            // 플레이어와 오브젝트 사이의 거리 계산
            float distance = Vector3.Distance(transform.position, player.position);

            // 거리가 attractionRange 이내라면 플레이어 방향으로 이동
            if (distance <= attractionRange)
            {
                // 플레이어 방향 계산
                Vector3 direction = (player.position - transform.position).normalized;

                // 플레이어 방향으로 이동
                transform.position += new Vector3(direction.x, 0f, direction.z) * attractionSpeed * Time.deltaTime;
            }

        }

        // 디버그용: 범위를 시각적으로 확인 (Scene View에서)
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attractionRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerInfoManager.Instance.AddGold(chargeGold);

                Destroy(gameObject);
            }

        }


     }
}