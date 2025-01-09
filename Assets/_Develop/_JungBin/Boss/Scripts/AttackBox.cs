using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// 공격시 켜지는 어택박스
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
                Debug.Log("맞음");
                player.TakeDamage(20);
            }
        }
    }
}