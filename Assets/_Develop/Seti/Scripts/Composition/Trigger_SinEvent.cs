using UnityEngine;
using Noah;
using System.Linq;

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

        [Header("Composition")]
        [SerializeField]
        private GameObject eventObject;
        #endregion

        // 이벤트 메서드
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (eventObject)
                {
                    ScenarioProgress progress = SaveLoadManager.Instance.scenarioSaveData.dialogueDatas.FirstOrDefault(data => data.ScenarioName == StoryManager.Instance.StageName);
                    if (progress != null)
                    {
                        if (!progress.CheckSeens[dialogueNumber])
                        {
                            Instantiate(eventObject, transform.position, Quaternion.identity, transform);
                        }
                    }
                }
                StoryManager.Instance.OpenDialogue(dialogueNumber);
            }
        }
    }
}