using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Seti
{
    /// <summary>
    /// Volume Controller
    /// </summary>
    public class Controller_Volume : MonoBehaviour
    {
        // 필드
        #region Variables
        // 기본
        private Volume volume;
        private Vignette vignette;

        // 객체
        private Player player;

        // 피격 연출
        private IEnumerator damageCor;
        [SerializeField]
        private float damageDuration = 1f;
        [SerializeField]
        private float damageIntensity = 0.7f;
        #endregion

        // 라이프 사이클
        private void Start()
        {
            // 참조
            volume = GetComponent<Volume>();
            player = FindAnyObjectByType<Player>();

            // 초기화
            Initialize();
        }

        // 메서드
        void Initialize()
        {
            if (player)
            {
                Damagable damagable = player.GetComponent<Damagable>();
                damagable.OnReceiveDamage += OnReceiveDamage;
            }

            if (volume)
            {
                volume.profile.TryGet(out vignette);
                if (vignette)
                {
                    Debug.Log($"Vignette 세팅 완료");
                }
                else
                {
                    Debug.LogWarning($"[Add Override]를 통해 [Vignette] 프로필을 추가해주세요.");
                }
            }
        }

        void OnReceiveDamage()
        {
            if (damageCor != null)
            {
                StopCoroutine(damageCor);
                damageCor = null;
            }

            damageCor = ReceiveDamageCor(damageDuration);
            StartCoroutine(damageCor);
        }

        // 유틸리티
        IEnumerator ReceiveDamageCor(float duration)
        {
            // 피격 시작
            vignette.intensity.value = damageIntensity;

            // 원상복귀
            float elapsed = 0f;
            float timeStamp = Time.time;
            while (timeStamp + duration > Time.time)
            {
                elapsed += Time.deltaTime;
                float sharpness = elapsed / duration;

                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0, Mathf.SmoothStep(0, 1, sharpness));
                yield return null;
            }

            // 피격 연출 완료
            vignette.intensity.value = 0f;
            yield break;
        }
    }
}