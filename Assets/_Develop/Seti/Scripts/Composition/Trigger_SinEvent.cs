using UnityEngine;
using Noah;

namespace Seti
{
    /// <summary>
    /// 원흉 이벤트 트리거
    /// </summary>
    public class Trigger_SinEvent : MonoBehaviour
    {
        // 필드
        #region Variables
        [Header("Variables")]
        [SerializeField]
        private int dialogueNumber;
        [SerializeField]
        private float dialogueDelay = 1f;

        [Header("Composition")]
        [SerializeField]
        private GameObject eventObject;
        #endregion

        // 이벤트 메서드
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (eventObject/* && !DataManager.Instance.DialogueData.CheckSeens[dialogueNumber]*/)
                {
                    Instantiate(eventObject, transform.position, Quaternion.identity, transform);
                }
                StoryManager.Instance.OpenDialogue(dialogueNumber);
            }
        }
    }
}