using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// 게임 스토리 총괄 디렉터
    /// </summary>
    public class StoryManager : Singleton<StoryManager>
    {
        // 필드
        // Input Action
        private InputSystem_Actions control;
        private DialogueUI dialogueUI;

        // 속성

        // 메서드

        // 필수 요소
        #region Require
        public Player Player { get; private set; }
        protected override void Awake()
        {
            base.Awake();

            // 초기화
            Initialize();
        }

        protected void OnEnable()
        {
            control.Player.Interact.started += OnNextDialogueStarted;
        }

        protected void OnDisable()
        {
            control.Player.Interact.started -= OnNextDialogueStarted;
        }

        // 이벤트 핸들러
        private void OnNextDialogueStarted(InputAction.CallbackContext _)
        {
            if (dialogueUI.nextButton.gameObject.activeSelf)
                dialogueUI.nextButton.onClick.Invoke();
        }

        private void Initialize()
        {
            control = new();

            Player = FindAnyObjectByType<Player>();
            if (!Player)
            {
                Debug.LogWarning("No Player, No Game.");
                return;
            }

            dialogueUI = FindAnyObjectByType<UIManager>().dialogueUI;
            dialogueUI.OnDialogueEnter += OnDisablePlayer;
            dialogueUI.OnDialogueEnd += OnEnablePlayer;
        }

        private void OnEnablePlayer()
        {
            Condition_Player condition_Player = Player.Condition as Condition_Player;
            condition_Player.PlayerSetActive(true);
        }

        private void OnDisablePlayer()
        {
            Condition_Player condition_Player = Player.Condition as Condition_Player;
            condition_Player.PlayerSetActive(false);
        }
        #endregion
    }
}