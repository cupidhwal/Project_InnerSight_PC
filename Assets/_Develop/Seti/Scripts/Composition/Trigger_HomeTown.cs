using System.Collections;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 마을 Dialogue 트리거
    /// </summary>
    public class Trigger_HomeTown : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField]
        private float dialogueDelay = 1f;

        public void OpenDialogue(int deathCount)
        {

            StartCoroutine(DialogueCor(deathCount));
        }

        IEnumerator DialogueCor(int deathCount)
        {
            yield return new WaitForSeconds(1);
            //StoryManager.Instance.OpenDialogue(dialogueNumber);
        }
    }
}