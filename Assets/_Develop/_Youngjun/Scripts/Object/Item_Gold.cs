using UnityEngine;

namespace Noah
{
    public class Item_Gold : MonoBehaviour
    {
        public int chargeGold = 10;
        private Transform effect_obj;
        public Transform player;        // �÷��̾� Transform
        private Vector3 startPos;

        public float attractionRange = 5f;  // �÷��̾���� ���� �Ÿ�
        public float attractionSpeed = 2f; // �÷��̾�� �̵��ϴ� �ӵ�

        private float amplitude = 0.2f;  // ������Ʈ�� �̵��� �ִ� �Ÿ�
        private float frequency = 2f;    // ������ �ӵ�

        private bool isTracking = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            startPos = transform.position;
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
            // �ð��� ���� y ��ġ�� sin �Լ��� ����
            float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

            // ���ο� ��ġ�� �̵�
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }

        void TrackingPlayer()
        {
            isTracking = true;

            // �÷��̾�� ������Ʈ ������ �Ÿ� ���
            float distance = Vector3.Distance(transform.position, player.position);

            // �Ÿ��� attractionRange �̳���� �÷��̾� �������� �̵�
            if (distance <= attractionRange)
            {
                // �÷��̾� ���� ���
                Vector3 direction = (player.position - transform.position).normalized;

                // �÷��̾� �������� �̵�
                transform.position += new Vector3(direction.x, 0f, direction.z) * attractionSpeed * Time.deltaTime;
            }

        }

        // ����׿�: ������ �ð������� Ȯ�� (Scene View����)
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attractionRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerInfoManager.Instance.GetGold(chargeGold);

                Destroy(gameObject);
            }

        }


     }
}