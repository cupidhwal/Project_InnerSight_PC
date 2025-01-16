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
                Debug.Log("기둥 부숴짐");
                BrokenWall brokenWall = other.transform.GetComponent<BrokenWall>();
                animator.SetBool("IsWall", true);
                brokenWall.RushToWall();
            }
        }
    }
}