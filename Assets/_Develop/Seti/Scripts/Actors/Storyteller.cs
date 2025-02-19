using UnityEngine;
using UnityEngine.Events;

namespace Seti
{
    /// <summary>
    /// 스토리에 필요한 NPC
    /// </summary>
    public abstract class Storyteller : MonoBehaviour
    {
        // 필드
        #region Variables
        // 이벤트
        public UnityAction OnStoryEnter;
        public UnityAction OnStoryEnd;
        protected DialogUI dialogUI;

        [Header("Story : Path")]
        [SerializeField]
        protected Dialogue dialogue;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 초기화
            Initialize();
        }

        private void Update()
        {
            // 조건 확인 후 이벤트 시작
            StoryEnter();
        }
        #endregion

        // 메서드
        #region Methods
        // 초기화
        protected virtual void Initialize()
        {
            // 참조
            dialogUI = Manager_Game.Instance.DialogUI;

            // 이벤트 구독
            OnStoryEnter += Dialogue;
            OnStoryEnter += DialogueSwitch;
        }

        // 이벤트
        public abstract void StoryEnter();
        public void StoryEnd() => OnStoryEnd?.Invoke();
        void Dialogue() => Manager_Game.Instance.DialogUI.SetDialogue(this, dialogue.xmlPath, dialogue.dialogueIndex);
        void DialogueSwitch() => dialogue.isRead = true;
        #endregion
    }
}