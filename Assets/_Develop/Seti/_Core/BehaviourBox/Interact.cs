using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// Move Behaviour
    /// </summary>
    [System.Serializable]
    public class Interact : IBehaviour
    {
        // 필드
        #region Variables
        // 전략 관리
        private Player player;
        #endregion

        // 인터페이스
        #region Interface
        // 초기화
        public void Initialize(Actor actor)
        {
            if (actor is Player)
                player = actor as Player;
        }

        public Type GetBehaviourType() => typeof(Interact);
        #endregion

        // 컨트롤러
        #region Controllers
        public void OnInteractStarted(InputAction.CallbackContext _) => OnInteraction();
        #endregion

        // 메서드
        #region Methods
        void OnInteraction()
        {
            if (StoryManager.Instance.IsDialogue)
            {
                StoryManager.Instance.NextDialogue();
                return;
            }

            if (player.CurrentTeller != null && player.CurrentTeller.CanDialogue)
            {
                if (!player.CurrentTeller.GetComponent<NPC_Life>().IsDead)
                {
                    bool check = false;
                    foreach (var dialogue in player.CurrentTeller.DialogueVariables)
                    {
                        if (DataManager.Instance.deathCount < dialogue.criteria_Death)
                            continue;

                        if (DataManager.Instance.sinEvent.Count(value => value) < dialogue.criteria_SinEvent)
                            continue;

                        if (DataManager.Instance.DialogueData.CheckSeens[dialogue.dialogueNumber])
                            continue;

                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (!check) return;
                    player.CurrentTeller.StoryEnter();
                    DataManager.Instance.UIManager.ToggleActionUI();
                }
            }

            if (player.CurrentNPC != null && player.CurrentNPC.Type != NPC_Type.Storyteller)
            {
                player.CurrentNPC.Switch_TradeUI();
                DataManager.Instance.UIManager.ToggleActionUI();
                return;
            }
        }
        #endregion
    }
}