using System.Collections;
using UnityEngine;

namespace Noah
{
    public class ObjectFadeSystem : Singleton<ObjectFadeSystem>
    {
        public float fadeDuration = 2f; // 페이드 지속 시간

        public void ObjectFadeIn_Paritcle(Transform _object/*, ParticleSystem[] _particleSystems*/)
        {
            ParticleSystem[] _particleSystems = _object.GetComponentsInChildren<ParticleSystem>(true);

            // 페이드 시작 전 모든 파티클 재생 멈추기 (투명한 상태에서 안보이게)
            foreach (ParticleSystem ps in _particleSystems)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            StartCoroutine(FadeIn_Paritcle(_particleSystems));
        }

        public void ObjectFadeIn_Object(Transform _object)
        {
            Renderer[] _renderers = _object.GetComponentsInChildren<Renderer>(true);

            StartCoroutine(FadeIn_Object(_renderers));
        }

        public void ObjectFadeIn_Canvas(Transform _object)
        {
            CanvasGroup[] _canvasGroups = _object.GetComponentsInChildren<CanvasGroup>(true);

            StartCoroutine(FadeIn_Canvas(_canvasGroups));
        }

        IEnumerator FadeIn_Paritcle(ParticleSystem[] _particleSystems)
        {
            float elapsedTime = 0f;
            SetAlpha_Paritcle(0f, _particleSystems); // 처음에는 완전히 투명

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                SetAlpha_Paritcle(alpha, _particleSystems);
                yield return null;
            }

            // 마지막 보정 (완전히 보이게 설정)
            SetAlpha_Paritcle(1f, _particleSystems);

            // 페이드 완료 후 파티클 다시 재생
            foreach (ParticleSystem ps in _particleSystems)
            {
                ps.Play();
            }
        }

        IEnumerator FadeIn_Object(Renderer[] _renderers)
        {
            float elapsedTime = 0f;
            SetAlpha_Object(0f, _renderers); // 처음에는 완전히 투명

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                SetAlpha_Object(alpha, _renderers);
                yield return null;
            }

            // 마지막 보정 (완전히 보이게 설정)
            SetAlpha_Object(1f, _renderers);
        }

        IEnumerator FadeIn_Canvas(CanvasGroup[] _canvasGroups)
        {
            float elapsedTime = 0f;
            SetAlpha_Canvas(0f, _canvasGroups); // 처음에는 완전히 투명

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                SetAlpha_Canvas(alpha, _canvasGroups);
                yield return null;
            }

            // 마지막 보정 (완전히 보이게 설정)
            SetAlpha_Canvas(1f, _canvasGroups);
        }


        void SetAlpha_Paritcle(float _alpha, ParticleSystem[] _particleSystems)
        {
            // 모든 ParticleSystem 투명도 변경 (colorOverLifetime 사용)
            foreach (ParticleSystem ps in _particleSystems)
            {
                var colorModule = ps.colorOverLifetime;

                if (!colorModule.enabled)
                    // colorOverLifetime 활성화
                    colorModule.enabled = true; 

                // 기존 colorOverLifetime 값을 유지하면서 알파 값만 변경
                Gradient grad = new Gradient();
                grad.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(_alpha, 1.0f) } // 처음은 투명, 나중에는 alpha
                );

                colorModule.color = new ParticleSystem.MinMaxGradient(grad);
            }
        }

        void SetAlpha_Object(float _alpha, Renderer[] _renderers)
        {
            // 모든 Renderer(메시, 스프라이트) 투명도 변경
            foreach (Renderer rend in _renderers)
            {
                foreach (Material mat in rend.materials) // 여러 개의 머터리얼 대응
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color color = mat.color;
                        color.a = _alpha;
                        mat.color = color;
                    }
                }
            }
        }

        void SetAlpha_Canvas(float _alpha, CanvasGroup[] _canvasGroups)
        {
            // UI(CanvasGroup) 투명도 변경
            foreach (CanvasGroup cg in _canvasGroups)
            {
                cg.alpha = _alpha;
            }
        }
    }
}
