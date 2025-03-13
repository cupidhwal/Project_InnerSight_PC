using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    [Serializable]
    public class TriggerVariables
    {
        public int deathCount;
        public int targetSinEvent = -1;
        public int dialogueNumber;
        public float dialogueDelay = 1f;
    }

    /// <summary>
    /// 스테이지의 이벤트 트리거
    /// </summary>
    public class Trigger_Stage : MonoBehaviour
    {
        [Header("Variables : Dialogue")]
        [SerializeField]
        protected List<TriggerVariables> triggers;

        public virtual void OpenDialogue()
        {
            int index = -1;
            float delay = 0;
            DialogueData dialogueData = DataManager.Instance.GetDialogData();
            foreach (var data in triggers)
            {
                // 기독 여부 체크
                if (dialogueData.CheckSeens[data.dialogueNumber])
                    continue;

                // 원흉 이벤트 체크
                if (data.targetSinEvent >= 0)
                {
                    if (DataManager.Instance.deathCount <= data.deathCount)
                        continue;

                    if (!DataManager.Instance.sinEvent[data.targetSinEvent])
                    {
                        bool check = false;
                        for (int i = 0; i < data.targetSinEvent; i++)
                        {
                            if (!DataManager.Instance.sinEvent[i])
                            {
                                check = true;
                                break;
                            }
                        }

                        if (!check)
                        {
                            index = data.dialogueNumber;
                            delay = data.dialogueDelay;
                            break;
                        }
                    }
                }
                else
                {
                    index = data.dialogueNumber;
                    delay = data.dialogueDelay;
                    break;
                }
            }

            // 실행 가능한 대화가 존재하면 출력
            if (index >= 0)
            {
                InitializeManager.Instance.Player.Condition.Action(false);
                StartCoroutine(DialogueCor(index, delay));
            }
        }

        IEnumerator DialogueCor(int dialogueNumber, float delay)
        {
            yield return new WaitForSeconds(delay);
            StoryManager.Instance.OpenDialogue(dialogueNumber);
            yield break;
        }
    }
}