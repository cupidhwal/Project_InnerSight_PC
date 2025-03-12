using Seti;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Noah
{
    public class SceneFade : MonoBehaviour
    {
        public static SceneFade instance;   

        [SerializeField] private Image fadeImage;
        [SerializeField] private Image startFadeImage;
        public AnimationCurve fadeCurve;

        public bool startFadeIn = true;

        public TMP_Text loadingText;  // UI Text 연결
        private string baseText = "Loading"; // 기본 텍스트
        private int dotCount = 0;
        private float fadeDuration = 1.5f; // 서서히 사라지는 시간

        private float fadeInDelay = 3f;
        private float fadeOutTime = 2f;

        Condition_Player condition_Player;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            condition_Player = FindAnyObjectByType<Condition_Player>();

            if (fadeImage.gameObject.activeSelf == false)
            {
                if (startFadeIn == true)
                {
                    fadeImage.gameObject.SetActive(true);

                    fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 255f);

                    if (SceneManager.GetActiveScene().name == "MainMenu")
                    {
                        StartCoroutine(FadeIn_Co(null, 0f, startFadeImage));
                    }
                    else
                    {
                        FadeIn(null);
                    }
                }
            }
        }

        public void FadeIn(string name, float delay = 3f)
        {
            StartCoroutine(FadeIn_Co(name, delay, fadeImage)); 
            //StartCoroutine(UpdateLoadingText());
        }

        public void FadeOut(string name, float delay = 0f)
        {
            StartCoroutine(FadeOut_Co(name, delay));
            StartCoroutine(UpdateLoadingText());
        }

        public void FadeOut(int name, float delay = 0f)
        {
            StartCoroutine(FadeOut_Co(name, delay));
        }


        IEnumerator FadeIn_Co(string name, float delay, Image _fadeImage)
        {
            float time = 1f;
            float ctime = 0f;

            _fadeImage.gameObject.SetActive(true);

            if (condition_Player != null)
                condition_Player.PlayerSetActive(false);

            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            while (ctime < time)
            {
                float a = fadeCurve.Evaluate(time);

                _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, a);
                time -= Time.deltaTime;
                yield return null;
            }

            loadingText.gameObject.SetActive(false);

            if (name != null)
            {
                SceneManager.LoadScene(name);
            }

            _fadeImage.gameObject.SetActive(false);

            if (condition_Player != null)
                condition_Player.PlayerSetActive(true);
        }

        IEnumerator FadeOut_Co(string name, float delay)
        {
            float time = fadeOutTime;
            float ctime = 0f;

            fadeImage.gameObject.SetActive(true);

            if(condition_Player != null)
                condition_Player.PlayerSetActive(false);

            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            while (ctime < time)
            {
                float a = fadeCurve.Evaluate(ctime);

                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, a);
                ctime += Time.deltaTime;
                yield return null;
            }

            if (name != null)
            {
                SceneManager.LoadScene(name);
            }
        }

        IEnumerator FadeOut_Co(int name, float delay)
        {
            float time = 1f;
            float ctime = 0f;

            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            while (ctime < time)
            {
                float a = fadeCurve.Evaluate(ctime);

                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, a);
                ctime += Time.deltaTime;
                yield return null;
            }

            if (name != null)
            {
                SceneManager.LoadScene(name);
            }

        }

        IEnumerator UpdateLoadingText()
        {
            loadingText.gameObject.SetActive(true);

            loadingText.color = new Color(255f, 255f, 255f, 255f);

            float elapsedTime = 0f;

            // 3초 동안 실행
            while (elapsedTime < (fadeInDelay + fadeOutTime + 1f))
            {
                dotCount = (dotCount + 1) % 4;  // 0, 1, 2, 3 순환
                loadingText.text = baseText + new string('.', dotCount);
                yield return new WaitForSeconds(1f);
                elapsedTime += 1f;
            }
            // 3초 후 서서히 사라지는 애니메이션 실행
            StartCoroutine(FadeOutText());
        }

        IEnumerator FadeOutText()
        {
            float startAlpha = loadingText.color.a;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
                loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, alpha);
                yield return null;
            }

            // 완전히 사라진 후 오브젝트 비활성화
            loadingText.gameObject.SetActive(false);
        }


    }
}
