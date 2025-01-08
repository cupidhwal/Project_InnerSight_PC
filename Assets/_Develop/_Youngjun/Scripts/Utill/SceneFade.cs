using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Noah
{
    public class SceneFade : MonoBehaviour
    {
        public static SceneFade instance;   

        [SerializeField] private Image fadeImage;
        public AnimationCurve fadeCurve;

        public bool startFadeIn = true;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            fadeImage.color = new Color(0f, 0f, 0f, 255f);

            if (fadeImage.gameObject.activeSelf == false)
            {
                fadeImage.gameObject.SetActive(true);

                if (startFadeIn == true)
                {
                    FadeIn(null);
                }

            }

        }

        public void FadeIn(string name, float delay = 0f)
        {
            StartCoroutine(FadeIn_Co(name, delay));
        }

        public void FadeOut(string name, float delay = 0f)
        {
            StartCoroutine(FadeOut_Co(name, delay));
        }

        public void FadeOut(int name, float delay = 0f)
        {
            StartCoroutine(FadeOut_Co(name, delay));
        }


        IEnumerator FadeIn_Co(string name, float delay)
        {
            float time = 1f;
            float ctime = 0f;

            if(delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

     

            while (ctime < time)
            {
                float a = fadeCurve.Evaluate(time);

                fadeImage.color = new Color(0f, 0f, 0f, a);
                time -= Time.deltaTime;
                yield return null;
            }

            if (name != null)
            {
                SceneManager.LoadScene(name);
            }

        }

        IEnumerator FadeOut_Co(string name, float delay)
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

                fadeImage.color = new Color(0f, 0f, 0f, a);
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

                fadeImage.color = new Color(0f, 0f, 0f, a);
                ctime += Time.deltaTime;
                yield return null;
            }

            if (name != null)
            {
                SceneManager.LoadScene(name);
            }

        }


    }
}
