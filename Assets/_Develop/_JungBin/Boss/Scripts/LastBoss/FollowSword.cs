using UnityEngine;

namespace JungBin
{
    [DefaultExecutionOrder(9999)]
    public class FollowSword : MonoBehaviour
    {
        // 필드
        [SerializeField] private Transform toFollow;      //부착할 대상

        // 라이프 사이클
        private void Start()
        {
        }

        private void Update()
        {
            transform.position = toFollow.position;
            transform.rotation = toFollow.rotation;

            //Debug.Log(toFollow.transform.position);

        }
    }
}