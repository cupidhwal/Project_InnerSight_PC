using UnityEngine;
namespace Seti
{
    /// <summary>
    /// 단역
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
        protected int dialogueNumber = 1;
        [SerializeField]
        protected float range_Event = 3f;
        [SerializeField]
        protected bool canDialogue = false;
        #endregion

        // 속성
        public bool CanDialogue => canDialogue;

        // 오버라이드
        public override void StoryEnter()
        {
            if (StoryManager.Instance.IsDialogue)
            {
                StoryManager.Instance.NextDialogue();
            }
            else
            {
                StoryManager.Instance.OpenDialogue(dialogueNumber);
            }
        }

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            // 초기화
            player = StoryManager.Instance.Player;
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