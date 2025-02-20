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
        private Condition_Player condition_Player;
        #endregion

        // 속성
        public int CurrentIndex => currentIndex;
        public string CurrentDialogue { get; private set; }
        public bool IsDialogue { get; private set; } = false;

        // 라이프 사이클
        private void Start()
        {
            StageManager.Instance.stageEndEvent += CheckCurrentStage;
        }

        // 대화
        private void SetDialogue(int index)
        {
            currentIndex = index;
            DataManager.GetDialogData();
        }
        public void OpenDialogue(int index)
        {
            uiManager.OpenDialogueUI(index);
        }
        public void NextDialogue()
        {
            uiManager.NextDialogueUI();
        }

        // 기타 메서드
        #region Methods
        public void LoadDialogue(int index) => currentIndex = index;

        private void CheckCurrentStage()
        {
            string stageName = StageManager.Instance.CurrentStage.name.Replace("Stage", "").Replace("(Clone)", "").Trim();
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

            // 초기화
            InitializeOnAwake();
        }

        private void InitializeOnAwake()
        {
            Player = FindAnyObjectByType<Player>();
            if (!Player)
            {
                Debug.LogWarning("No Player, No Game.");
                return;
            }
            condition_Player = Player.Condition as Condition_Player;

            uiManager = FindAnyObjectByType<UIManager>();
            uiManager.dialogueUI.OnDialogueEnter += OnDisablePlayer;
            uiManager.dialogueUI.OnDialogueEnd += OnEnablePlayer;

            CurrentDialogue = dialogueList[currentIndex];
        }

        private void OnEnablePlayer()
        {
            condition_Player.PlayerSetActive(true);
            IsDialogue = false;
        }

        private void OnDisablePlayer()
        {
            condition_Player.PlayerSetActive(false);
            IsDialogue = true;
        }
        #endregion
    }
}