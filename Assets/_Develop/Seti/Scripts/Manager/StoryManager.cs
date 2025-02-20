using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Noah;

namespace Seti
{
    /// <summary>
    /// 게임 스토리 총괄 디렉터
    /// </summary>
    public class StoryManager : Singleton<StoryManager>
    {
        // 필드
        #region Variables
        // 스토리
        [SerializeField]
        private int currentIndex;
        [SerializeField]
        private List<string> dialogueList = new();

        // 참조
        private UIManager uiManager;

        // Input Action
        private InputSystem_Actions control;
        #endregion

        // 속성
        public int CurrentIndex => currentIndex;
        public string CurrentDialogue { get; private set; }

        // 라이프 사이클
        private void Start()
        {
            CurrentDialogue = dialogueList[currentIndex];

            // 초기화
            Initialize();
        }

        // 대화
        public void OpenDialogue(int index)
        {
            Debug.Log($"StoryManager.OpenDialogue");
            uiManager.OpenDialogueUI(index);
        }
        private void SetDialogue(int index)
        {
            currentIndex = index;
            DataManager.GetDialogData();
        }

        // 기타 메서드
        #region Methods
        public void LoadDialogue(int index) => currentIndex = index;

        private void CheckCurrentStage()
        {
            Debug.Log($"CheckCurrentStage");

            string stageName = StageManager.Instance.gameObject.GetComponentInChildren<GameObject>().name.Replace("Stage", "");
            switch (stageName)
            {
                case "_T":
                    SetDialogue(0);
                    OpenDialogue(0);
                    break;
            }
        }
        #endregion

        // 필수 요소
        #region Require
        public Player Player { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            control = new();
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
            if (uiManager.dialogueUI.nextButton.gameObject.activeSelf)
                uiManager.dialogueUI.nextButton.onClick.Invoke();
        }

        private void Initialize()
        {
            Debug.Log($"StoryManager.Initialize");

            Player = FindAnyObjectByType<Player>();
            if (!Player)
            {
                Debug.LogWarning("No Player, No Game.");
                return;
            }

            uiManager = FindAnyObjectByType<UIManager>();
            uiManager.dialogueUI.OnDialogueEnter += OnDisablePlayer;
            uiManager.dialogueUI.OnDialogueEnd += OnEnablePlayer;

            StageManager.Instance.stageStartEvent += CheckCurrentStage;

            Debug.Log($"StoryManager.Initialize.Clear");
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