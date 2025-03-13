using JungBin;
using System.Linq;
using UnityEngine;

namespace Seti
{
    [RequireComponent(typeof(Condition_Player))]
    /// <summary>
    /// Player
    /// </summary>
    public class Player : Actor
    {
        // View type
        public enum ViewType
        {
            QuaterView,
            Follow_Person,
        }

        // 필드
        #region Variables
        [Header("Variables: Dash")]
        [SerializeField]
        private float dashSpeed = 2f;
        [SerializeField]
        private float dashCooldown = 1f;
        [SerializeField]
        private float dashDuration = 0.2f;

        [Header("View Type")]
        [SerializeField]
        private ViewType viewType;
        public ViewType View => viewType;
        #endregion

        // 속성
        #region Properties
        // Player 전용
        public float Dash_Speed => dashSpeed;
        public float Dash_Cooldown => dashCooldown;
        public float Dash_Duration => dashDuration;
        #endregion

        // 상호작용
        #region Interaction
        [SerializeField]
        private NPC currentNPC;
        public NPC CurrentNPC => currentNPC;
        public void SetNPC(NPC npc)
        {
            currentNPC = npc;

            string info;
            if (currentNPC)
            {
                info = currentNPC.Type switch
                {
                    NPC_Type.Enhance => "스탯 강화",
                    NPC_Type.Trinkets => "유물 교체",
                    _ => "대화"
                };
            }
            else
            {
                DataManager.Instance.UIManager.CloseActionUI();
                return;
            }

            if (info == "유물 교체" && RelicManager.Instance.GetRelics().Count == 0)
                return;

            if (info == "대화")
            {
                if (currentNPC.GetComponent<NPC_Life>().IsDead)
                    return;

                bool check = false;
                foreach (var dialogue in currentNPC.GetComponent<Storyteller_NPC>().DialogueVariables)
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
            }

            DataManager.Instance.UIManager.SwitchActionUI(true, info);
        }
        [SerializeField]
        private Storyteller_NPC storyteller;
        public Storyteller_NPC CurrentTeller => storyteller;
        public void SetTeller(Storyteller_NPC teller) => storyteller = teller;
        #endregion

        // 오버라이드
        #region Override
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_Player>();
        #endregion

        public void DashSpeed(float speed)
        {
            dashSpeed = speed;
        }
    }
}