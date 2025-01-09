using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// ���ݽ� ������ ���ùڽ�
    /// </summary>
    public class AttackBox : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;

        [SerializeField] private Player player;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<BoxCollider>() == boxCollider)
            {
                Debug.Log("����");
                player.TakeDamage(20);
            }
        }
    }
}