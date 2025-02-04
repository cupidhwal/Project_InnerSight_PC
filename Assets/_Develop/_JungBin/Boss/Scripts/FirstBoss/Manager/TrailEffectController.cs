using UnityEngine;
using System.Collections;

namespace JungBin
{
    public class TrailEffectController : MonoBehaviour
    {
        [Header("Trail Settings")]
        [SerializeField] private GameObject trailPrefab; // 잔상을 나타낼 프리팹
        [SerializeField] private float trailSpawnInterval = 0.1f; // 잔상 생성 간격

        private bool isCreatingTrail = false;
        private GameObject activeTrail;

        /// <summary>
        /// 잔상 효과를 시작합니다.
        /// </summary>
        public void StartTrailEffect()
        {
            if (!isCreatingTrail)
            {
                isCreatingTrail = true;
                StartCoroutine(EnableTrail());
            }
        }

        /// <summary>
        /// 잔상 효과를 종료합니다.
        /// </summary>
        public void StopTrailEffect()
        {
            isCreatingTrail = false;
            if (activeTrail != null)
            {
                activeTrail.SetActive(false);
            }
        }

        private IEnumerator EnableTrail()
        {
            while (isCreatingTrail)
            {
                if (activeTrail == null)
                {
                    // Trail Prefab 초기화 및 활성화
                    activeTrail = Instantiate(trailPrefab, transform.position, transform.rotation, this.transform);
                    activeTrail.SetActive(true);
                }
                else
                {
                    // Trail 위치 및 회전 업데이트
                    activeTrail.transform.position = transform.position;
                    activeTrail.transform.rotation = transform.rotation;
                    activeTrail.transform.localScale = transform.localScale;
                    activeTrail.SetActive(true);
                }

                // 생성 간격 대기
                yield return new WaitForSeconds(trailSpawnInterval);
            }

            // 잔상 비활성화
            if (activeTrail != null)
            {
                activeTrail.SetActive(false);
            }
        }
    }
}
