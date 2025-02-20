using UnityEngine;

namespace JungBin
{
    public class SpikeChild : MonoBehaviour
    {
        private SpikeController parentSpike; // 부모 참조

        private void Start()
        {
            parentSpike = GetComponentInParent<SpikeController>(); // 부모 찾기
        }

        private void OnTriggerEnter(Collider other)
        {
            if (parentSpike != null) // 부모가 존재하는 경우만 실행
            {
                parentSpike.OnChildTriggerEnter(other); // 부모에게 이벤트 전달
            }
        }
    }
}
