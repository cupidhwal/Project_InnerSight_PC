using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Fade Action", menuName = "Scenario/Composition/Scene/Fade")]
    public class Composition_Fade : CompositionObject
    {
        private enum ActiveFlag
        {
            In,
            Out
        }

        // 연출
        [Header("Variables")]
        [SerializeField]
        ActiveFlag activeFlag;
        [SerializeField]
        float excuteDuration = 1f;
        [SerializeField]
        float excuteSharpness = 10f;

        public override void Execute(GameObject _)
        {
            StoryManager.Instance.CorExcutor(Fade());
        }

        IEnumerator Fade()
        {
            StoryManager.Instance.IsComposition = true;

            // Set
            Image fadeImage = StoryManager.Instance.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            if (activeFlag == ActiveFlag.Out)
                fadeImage.gameObject.SetActive(true);

            // Variables
            float previousAlpha = 0f;
            float presentAlpha = 0f;

            // Select
            switch (activeFlag)
            {
                case ActiveFlag.In:
                    previousAlpha = 1f;
                    presentAlpha = 0f;
                    break;

                case ActiveFlag.Out:
                    previousAlpha = 0f;
                    presentAlpha = 1f;
                    break;
            }
            ColorUtility.SetAlpha(fadeImage, previousAlpha);

            // Fade
            float timeStamp = Time.time;
            while (timeStamp + excuteDuration > Time.time)
            {
                previousAlpha = Mathf.Lerp(previousAlpha, presentAlpha, excuteSharpness * Time.deltaTime);
                ColorUtility.SetAlpha(fadeImage, previousAlpha);
                yield return null;
            }

            // End
            if (activeFlag == ActiveFlag.In)
                fadeImage.gameObject.SetActive(false);

            StoryManager.Instance.IsComposition = false;
            yield break;
        }
    }
}