using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Seti;

namespace Yoon
{
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        #region Variables
        public Transform healthBarPivot;  // 헬스바가 카메라를 바라보도록 설정
        public Slider EnemyHealthBarSlider; // 헬스바 슬라이더

        private Damagable damagable; // Damagable 참조

        [SerializeField] private bool hideFullHealthBar = true; // 체력이 가득 찼을 때 숨길지 여부
        private float targetHealth; // 목표 체력 값
        private float lerpSpeed = 0.5f; // 체력 감소 속도
        private bool firstHit = false; // 처음 피격 여부 확인
        #endregion

        private void Start()
        {
            damagable = GetComponent<Damagable>();
            targetHealth = damagable.CurrentHitPoints;

            // 헬스바 초기 숨김
            if (hideFullHealthBar)
            {
                EnemyHealthBarSlider.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (damagable == null) return;

            // 체력이 가득 찼을 때 헬스바 숨김
            if (hideFullHealthBar)
            {
                EnemyHealthBarSlider.gameObject.SetActive(damagable.CurrentHitPoints < damagable.MaxHitPoint);
            }

            // 처음 피격 시 바로 체력 반영 (애니메이션 없이)
            if (!firstHit && damagable.CurrentHitPoints < damagable.MaxHitPoint)
            {
                firstHit = true;
                EnemyHealthBarSlider.value = damagable.CurrentHitRate; // 바로 설정 (Lerp 사용 X)
                return;
            }

            // 체력 변경 감지 후 애니메이션 적용
            if (targetHealth != damagable.CurrentHitPoints)
            {
                targetHealth = damagable.CurrentHitPoints;
                StartCoroutine(LerpHealthBar());
            }

            if (healthBarPivot != null)
            {
                healthBarPivot.rotation = Quaternion.Euler(0f, 45f, 0f);
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
        /*
                #region Variables
                public Transform healthBarPivot;                          // 헬스바가 카메라를 바라보도록 설정
                public Slider EnemyHealthBarSlider;                       // 헬스바 슬라이더

                private Damagable damagable;                             // Damagable 참조

                [SerializeField] private bool hideFullHealthBar = true;  // 체력이 가득 찼을 때 숨길지 여부
                private float targetHealth;                              // 목표 체력 값
                private float lerpSpeed = 0.5f;                          // 체력 감소 속도
                private bool firstHit = false;                           // 처음 피격 여부 확인
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


                    if (healthBarPivot != null)
                    {
                        // 부모 회전 고정 (Quarter View 방향 유지)
                        healthBarPivot.rotation = Quaternion.Euler(0f, 45f, 0f);  // 45도로 설정하여 정면을 보게 하기
        *//*
                        // 자식 UI 회전 초기화 (UI 요소가 회전하지 않도록)
                        foreach (Transform child in healthBarPivot)
                        {
                            child.localRotation = Quaternion.identity; // 자식의 회전 초기화 (UI 요소가 회전하지 않도록)

                            // UI 텍스트나 이미지가 뒤집히지 않도록 로컬 스케일 보정
                            Vector3 fixedScale = child.localScale;
                            fixedScale.x = Mathf.Abs(fixedScale.x); // X축 스케일 양수로 유지
                            child.localScale = fixedScale;

                        }*/


        /*// Health bar의 회전을 quarter view 기준으로 고정
        healthBarPivot.rotation = Quaternion.Euler(0f, 45f, 0f);

        // UI가 뒤집히는 문제 방지: 부모가 회전하면서 자식 UI가 반대로 회전되도록 조정
        foreach (Transform child in healthBarPivot)
        {
            child.rotation = Quaternion.identity; // 자식의 로컬 회전값을 초기화
        }*/

        /*  // 카메라와 health bar의 방향 벡터 계산
          Vector3 direction = Camera.main.transform.position - healthBarPivot.position;

          // y축 회전만 유지하도록 방향 벡터 수정
          direction.y = 0f;

          // 수정된 방향 벡터를 기준으로 healthBarPivot을 회전시킴
          //healthBarPivot.rotation = Quaternion.LookRotation(direction);
      *//*
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
*/
    }
}

