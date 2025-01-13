using UnityEngine;

namespace JungBin
{

    public class RushController : MonoBehaviour
    {
        [SerializeField] Animator animator;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Wall"))
            {
                animator.SetBool("IsWall", true);
            }
        }
    }
}