using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// 단역
    /// </summary>
    public class Storyteller_NPC : Storyteller
    {
        // 필드
        #region Variables
        private InputSystem_Actions control;

        [Header("Criteria : AI Behaviour")]
        protected Player player;
        [SerializeField]
        protected int dialogueNumber = 1;
        [SerializeField]
        protected float range_Event = 2f;
        [SerializeField]
        protected float distanceToPlayer = 0f;
        [SerializeField]
        protected bool canDialogue = false;
        #endregion

        // 초기화
        protected override void Initialize()
        {
            base.Initialize();
            player = StoryManager.Instance.Player;
        }

        // 이벤트 - Update
        public override void StoryEnter()
        {
            // 거리 계산
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer < range_Event)
            {
                canDialogue = true;
            }
            else canDialogue = false;
        }
        public override int DialogueNumber() => dialogueNumber;
    }
}