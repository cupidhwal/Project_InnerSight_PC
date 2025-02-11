using UnityEngine;

namespace JungBin
{

    public class RushController : MonoBehaviour
    {
        [SerializeField] Animator animator;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BWall"))
            {
                Debug.Log("기둥 부숴짐");
                BrokenWall brokenWall = other.transform.GetComponent<BrokenWall>();
                animator.SetBool("IsBWall", true);
                animator.SetBool("IsWall", true);
                brokenWall.RushToWall();
            }
            if (other.CompareTag("Player"))
            {
                animator.SetBool("IsPlayer", true);
            }
            if (other.CompareTag("Wall"))
            {
                animator.SetBool("IsWall", true);
            }
        }
    }
}