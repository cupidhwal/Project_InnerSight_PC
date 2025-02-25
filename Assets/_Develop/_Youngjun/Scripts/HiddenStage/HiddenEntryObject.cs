using System.Collections;
using UnityEngine;

namespace Noah
{ 
    public class HiddenEntryObject : MonoBehaviour
    {
        public float riseSpeed = 2f; // 상승 속도 설정
        public GameObject hiddenParitcle;
        
        private Vector3 paritclePos;

        void Start()
        {
            StartCoroutine(RiseToZero());        
        }

        IEnumerator RiseToZero()
        {
            GameObject paritcle = Instantiate(hiddenParitcle, new Vector3(transform.position.x, 0f, transform.position.z), Quaternion.identity);

            while (transform.position.y < -0.5f)
            {
                transform.position += Vector3.up * riseSpeed * Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }

            // y = 0 보정
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

            transform.GetComponent<Collider>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);

            paritcle.GetComponent<ParticleSystem>().Stop();

            yield return new WaitForSeconds(1f);

            transform.GetChild(0).GetComponent<Collider>().enabled = true;

            Destroy(paritcle);
        }
    }
    
}
