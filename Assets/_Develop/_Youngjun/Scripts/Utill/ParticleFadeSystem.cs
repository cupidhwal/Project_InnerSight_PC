using System.Collections;
using UnityEngine;

namespace Noah
{
    public class ParticleFadeSystem : Singleton<ParticleFadeSystem>
    {
        public float fadeDuration = 2f; // 페이드 지속 시간
        private Renderer[] renderers;
        private ParticleSystem[] particleSystems;
        private CanvasGroup[] canvasGroups;

        void Start()
        {
            // 자기 자신 + 모든 자식 오브젝트에서 Renderer, ParticleSystem, CanvasGroup 가져오기
            renderers = GetComponentsInChildren<Renderer>(true);
            particleSystems = GetComponentsInChildren<ParticleSystem>(true);
            canvasGroups = GetComponentsInChildren<CanvasGroup>(true);

            StartCoroutine(FadeIn());
        }

        public void ParticleFadeIn()
        { 

        }

        IEnumerator FadeIn()
        {
            float elapsedTime = 0f;
            SetAlpha(0f); // 초기 알파값 설정 (완전 투명)

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                SetAlpha(alpha);
                yield return null;
            }

            // 마지막 보정 (완전히 보이게 설정)
            SetAlpha(1f);
        }

        void SetAlpha(float alpha)
        {
            //// 1️⃣ 모든 Renderer(메시, 스프라이트) 투명도 변경
            //foreach (Renderer rend in renderers)
            //{
            //    foreach (Material mat in rend.materials)
            //    {
            //        if (mat.HasProperty("_Color"))
            //        {
            //            Color color = mat.color;
            //            color.a = alpha;
            //            mat.color = color;
            //        }
            //    }
            //}

            // 2️⃣ 모든 ParticleSystem 투명도 변경
            foreach (ParticleSystem ps in particleSystems)
            {
                var main = ps.main;
                main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, alpha);
            }

            //// 3️⃣ UI(CanvasGroup) 투명도 변경
            //foreach (CanvasGroup cg in canvasGroups)
            //{
            //    cg.alpha = alpha;
            //}
        }
    }
}