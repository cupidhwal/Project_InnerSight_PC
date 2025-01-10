using UnityEngine;

namespace JungBin
{

    public class SpawnAxeController : MonoBehaviour
    {
        [SerializeField] private int attackDamage = 0;
        private Transform goalPosition;
        [SerializeField] private float axeSpeed = 10f;
        private Vector3 dir;

        private void Start()
        {
            goalPosition = GameManager.Instance.PlayerTransform;
            dir = (goalPosition.position - transform.position).normalized;
        }

        private void Update()
        {
            transform.position += dir * axeSpeed * Time.deltaTime;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CapsuleCollider>() == GameManager.Instance.Player_HitCapsuleCollider)
            {
                Debug.Log("¸ÂÀ½");
                GameManager.Instance.Player.TakeDamage(attackDamage);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }

        }
    }
}