using System.Collections;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 스테이지의 이벤트 트리거
    /// </summary>
    public class Trigger_Stage : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField]
        private int dialogueNumber;
        [SerializeField]
        private float dialogueDelay = 1f;

        public void OpenDialogue() => StartCoroutine(DialogueCor());

        IEnumerator DialogueCor()
        {
            yield return new WaitForSeconds(1);
            StoryManager.Instance.OpenDialogue(dialogueNumber);
        }
    }
}