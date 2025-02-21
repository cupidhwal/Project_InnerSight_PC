using System.Collections;
using UnityEngine;

namespace Noah
{ 
    public class HiddenEntryObject : MonoBehaviour
    {
        public float riseSpeed = 2f; // 상승 속도 설정

        void Start()
        {
            StartCoroutine(RiseToZero());
        }

        IEnumerator RiseToZero()
        {
            while (transform.position.y < 0)
            {
                transform.position += Vector3.up * riseSpeed * Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }

            // y = 0 보정
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            transform.GetComponent<Collider>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    
}
