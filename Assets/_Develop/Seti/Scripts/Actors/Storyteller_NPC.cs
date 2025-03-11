using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Seti
{
    [Serializable]
    public struct DialogueVariables
    {
        public int criteria_Death;
        public int criteria_SinEvent;
        public int dialogueNumber;
    }

    /// <summary>
    /// NPC - 스토리 진행 클래스
    /// </summary>
    public class Storyteller_NPC : Storyteller
    {
        // 필드
        #region Variables
        protected Player player;

        [Header("Criteria : AI Behaviour")]
        [SerializeField]
        protected float distanceToPlayer = 0f;
        [SerializeField]
        protected bool canDialogue = false;

        [Header("Variables : Dialogue")]
        [SerializeField]
        protected List<DialogueVariables> dialogueVariables;
        #endregion

        // 속성
        public List<DialogueVariables> DialogueVariables => dialogueVariables;
        public bool CanDialogue => canDialogue;

        // 오버라이드
        public override bool StoryEnter()
        {
            if (StoryManager.Instance.IsDialogue)
            {
                StoryManager.Instance.NextDialogue();
                return true;
            }
            else
            {
                foreach (var dialogue in dialogueVariables)
                {
                    if (DataManager.Instance.deathCount < dialogue.criteria_Death)
                        continue;

                    if (DataManager.Instance.sinEvent.Count(value => value) < dialogue.criteria_SinEvent)
                        continue;

                    return StoryManager.Instance.OpenDialogue(dialogue.dialogueNumber);
                }
            }
            return false;
        }

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            // 초기화
            player = InitializeManager.Instance.Player;
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canDialogue = true;
                player.SetTeller(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canDialogue = false;
                player.SetTeller(null);
            }
        }
        #endregion
    }
}