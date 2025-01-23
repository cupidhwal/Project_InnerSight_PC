using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Seti;

namespace Yoon
{
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        #region Variables
        public Transform healthBarPivot;                          // 헬스바가 카메라를 바라보도록 설정
        public Slider EnemyHealthBarSlider;                       // 헬스바 슬라이더

        private Damagable damagable;                            // Damagable 참조

        [SerializeField] private bool hideFullHealthBar = true; // 체력이 가득 찼을 때 숨길지 여부
        private float targetHealth;                              // 목표 체력 값
        private float lerpSpeed = 0.5f;                            // 체력 감소 속도
        #endregion

        private void Start()
        {
            damagable = GetComponent<Damagable>();
            targetHealth = damagable.CurrentHitPoints;
        }

        private void Update()
        {
            // 체력이 가득 찼을 때 헬스바 숨김
            if (hideFullHealthBar && damagable != null)
            {
                EnemyHealthBarSlider.gameObject.SetActive(damagable.CurrentHitPoints < damagable.MaxHitPoint);
            }

            // 목표 체력 값이 변화한 경우에만 코루틴을 실행
            if (targetHealth != damagable.CurrentHitPoints)
            {
                targetHealth = damagable.CurrentHitPoints;
                StartCoroutine(LerpHealthBar());
            }

            // 헬스바가 카메라를 바라보도록 설정
            if (healthBarPivot != null)
            {
                healthBarPivot.LookAt(Camera.main.transform.position);
                healthBarPivot.rotation = Quaternion.Euler(0f, healthBarPivot.rotation.eulerAngles.y, 0f);
            }
        }

        // 체력 값을 서서히 변경하는 코루틴
        private IEnumerator LerpHealthBar()
        {
            float startHealth = EnemyHealthBarSlider.value;
            float endHealth = damagable.CurrentHitRate; // 체력 비율

            float elapsedTime = 0f;
            while (elapsedTime < lerpSpeed)
            {
                elapsedTime += Time.deltaTime;
                EnemyHealthBarSlider.value = Mathf.Lerp(startHealth, endHealth, elapsedTime / lerpSpeed);
                yield return null;
            }

            // 최종값을 정확히 설정
            EnemyHealthBarSlider.value = endHealth;
        }
    }
}
