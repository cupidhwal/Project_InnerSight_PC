using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Cinemachine;
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
        [Header("Dialogue")]
        [SerializeField]
        private int currentIndex;
        [SerializeField]
        private List<string> dialogueList = new();

        // 참조
        private UIManager uiManager;
        private Condition_Player condition_Player;

        // 연출
        [Header("Composition")]
        [SerializeField]
        private List<CompositionsPerScene> compositionList;
        #endregion

        // 속성
        #region Properties
        public Player Player { get; private set; }
        public CinemachineCamera Cinemachine { get; private set; }
        public GameObject TempTarget { get; private set; }
        public string CurrentDialogue { get; private set; }
        public string StageName { get; private set; }
        public bool IsDialogue { get; private set; } = false;
        #endregion

        // 라이프 사이클
        private void Start()
        {
            StageManager.Instance.stageEndEvent += SwitchCurrentStage;
        }

        // 대화
        private void SetDialogue(int index)
        {
            currentIndex = index;
            CurrentDialogue = dialogueList[currentIndex];
            DataManager.Instance.GetDialogData();
        }
        public void OpenDialogue(int index) => uiManager.OpenDialogueUI(index);
        public void NextDialogue() => uiManager.NextDialogueUI();

        // 연출
        public void SetTempTarget(GameObject tempTarget) => TempTarget = tempTarget;
        public void CorStopper() => StopAllCoroutines();
        public void CorExcutor(IEnumerator cor) => StartCoroutine(cor);
        public void SelectComposition(int number, int order)
        {
            string number_order = number.ToString() + "/" + order.ToString();
            var composition = compositionList[currentIndex].compositions.FirstOrDefault(com => com.ID == number_order);

            foreach (var act in composition.Actions)
            {
                act.Execute(composition.Target);
            }
        }
        public void ReadyComposition()
        {
            // 마을 포탈
            GameObject portals = StageManager.Instance.CurrentStage.transform.GetChild(0).GetChild(0).gameObject;
            Debug.Log($"portals: {portals}");
            DisableComposition("Stage000", 1, portals);

            // 미니맵
            GameObject miniMap = FindAnyObjectByType<Mini_Map>().gameObject;
            DisableComposition("Stage001", 0, miniMap);
        }
        private void DisableComposition(string stageName, int dialogueIndex, GameObject target)
        {
            ScenarioData data = SaveLoadManager.Instance.scenarioSaveData;
            if (data == null) return;

            ScenarioProgress progress = data.dialogueDatas.FirstOrDefault(d => d.ScenarioName == stageName);
            if (!data.dialogueDatas.Contains(progress) || !progress.CheckSeens[dialogueIndex])
            {
                target.SetActive(false);
            }
        }

        // 기타 메서드
        #region Methods
        private void SwitchCurrentStage()
        {
            StageName = StageManager.Instance.CurrentStage.name.Replace("(Clone)", "").Trim();
            switch (StageName)
            {
                case "Stage_T":
                    SetDialogue(0);
                    OpenDialogue(0);
                    break;

                case "Stage000":
                    SetDialogue(1);
                    OpenDialogue(0);
                    break;

                case "Stage001":
                    SetDialogue(2);
                    OpenDialogue(0);
                    break;
            }
        }
        #endregion

        // 필수 요소
        #region Require
        protected override void Awake()
        {
            base.Awake();

            // 초기화
            InitializeOnAwake();
        }

        private void InitializeOnAwake()
        {
            // 참조
            Player = FindAnyObjectByType<Player>();
            if (!Player)
            {
                Debug.LogWarning("No Player, No Game.");
                return;
            }
            condition_Player = Player.GetComponent<Condition_Player>();
            Cinemachine = FindAnyObjectByType<CinemachineCamera>();

            // 대화 이벤트 관리
            uiManager = DataManager.Instance.UIManager;
            uiManager.dialogueUI.OnDialogueEnter += OnDisablePlayer;
            uiManager.dialogueUI.OnDialogueEnd += OnEnablePlayer;

            CurrentDialogue = dialogueList[currentIndex];
        }

        private void OnEnablePlayer()
        {
            if (condition_Player == null) return;
            condition_Player.PlayerSetActive(true);
            IsDialogue = false;
        }

        private void OnDisablePlayer()
        {
            if (condition_Player == null) return;
            condition_Player.PlayerSetActive(false);
            IsDialogue = true;
        }

        private void OnValidate()
        {
            for (int i = 0; i < compositionList.Count; i++)
            {
                compositionList[i].UpdateIndex(i);
            }
        }
        #endregion
    }
}