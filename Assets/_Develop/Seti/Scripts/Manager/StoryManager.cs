using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Cinemachine;
using Noah;
using JungBin;

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
        private Composition currentComposition;
        [SerializeField]
        private List<CompositionsPerScene> compositionList;

        GameObject miniMap;
        #endregion

        // 속성
        #region Properties
        public Player Player { get; private set; }
        public CinemachineCamera Cinemachine { get; private set; }
        public Composition CurrentComp => currentComposition;
        public GameObject TempTarget { get; private set; }
        public string CurrentDialogue { get; private set; }
        public string StageName { get; private set; }
        public bool IsDialogue { get; private set; } = false;
        public bool IsComposition { get; set; } = false;
        #endregion

        // 대화
        private void SetDialogue(int index)
        {
            currentIndex = index;
            CurrentDialogue = dialogueList[currentIndex];
            DataManager.Instance.GetDialogData();
        }
        public bool OpenDialogue(int index)
        {
            ScenarioData data = SaveLoadManager.Instance.scenarioSaveData;
            if (data == null) return false;

            if (DataManager.Instance.DialogueData.CheckSeens[index])
                return false;

            uiManager.OpenDialogueUI(index);
            return true;
        }
        public void NextDialogue()
        {
            if (IsComposition) return;

            uiManager.NextDialogueUI();
        }

        // 연출
        public void SetTarget(GameObject target) => TempTarget = target;
        public void CorStopper() => StopAllCoroutines();
        public void CorExcutor(IEnumerator cor) => StartCoroutine(cor);
        public void SelectComposition(int number, int order)
        {
            string number_order = number.ToString() + "/" + order.ToString();
            currentComposition = compositionList[currentIndex].compositions.FirstOrDefault(com => com.ID == number_order);

            foreach (var act in currentComposition.Actions)
            {
                act.Execute(currentComposition.Target);
            }
        }
        public void ReadyComposition()
        {
            StageName = StageManager.Instance.CurrentStage.name.Replace("(Clone)", "").Trim();

            // 마을 포탈
            GameObject portals = StageManager.Instance.CurrentStage.transform.GetChild(0).gameObject;
            if (StageName == "Stage_T")
            {
                DisableComposition("Stage_T", 3, portals);
            }

            if (StageName == "Stage000")
            {
                
                if (SaveLoadManager.Instance.scenarioSaveData.deathCount >= 1)
                {
                    DisableComposition("Stage000", 1, portals);

                    if (SaveLoadManager.Instance.scenarioSaveData.sinEvent[0] ||
                        SaveLoadManager.Instance.scenarioSaveData.sinEvent[1])
                    {
                        if (SaveLoadManager.Instance.scenarioSaveData.deathCount >= 2)
                            DisableComposition("Stage000", 2, portals);
                        if (SaveLoadManager.Instance.scenarioSaveData.deathCount >= 3)
                            DisableComposition("Stage000", 3, portals);
                        if (SaveLoadManager.Instance.scenarioSaveData.deathCount >= 4)
                            DisableComposition("Stage000", 4, portals);
                        if (SaveLoadManager.Instance.scenarioSaveData.deathCount >= 5)
                            DisableComposition("Stage000", 5, portals);
                    }
                }
            }

            // 미니맵
            DisableComposition("Stage001", 0, miniMap);
        }
        private void DisableComposition(string stageName, int dialogueIndex, GameObject target)
        {
            ScenarioData data = SaveLoadManager.Instance.scenarioSaveData;
            if (data == null) return;

            ScenarioProgress progress = data.dialogueDatas.FirstOrDefault(d => d.ScenarioName == stageName);
            if (progress == null || !progress.CheckSeens[dialogueIndex])
            {
                target.SetActive(false);
            }
        }

        // 기타 메서드
        #region Methods
        private void InvokeStage() => Invoke(nameof(SwitchCurrentStage), 0f);
        private void SwitchCurrentStage()
        {
            if (IsDialogue) return;

            switch (StageName)
            {
                case "Stage_T":
                    SetDialogue(0);
                    OpenDialogue(0);
                    break;

                case "Stage000":
                    SetDialogue(1);
                    DialogueData townData = DataManager.Instance.GetDialogData();
                    if (DataManager.Instance.deathCount == 1)
                    {
                        if (townData != null && !townData.CheckSeens[0])
                            OpenDialogue(0);
                    }
                    else if (DataManager.Instance.deathCount >= 5)
                    {
                        if (townData != null && !townData.CheckSeens[5] && SaveLoadManager.Instance.scenarioSaveData.flynneEvent[4])
                            OpenDialogue(5);
                    }
                    break;

                case "Stage001":
                    SetDialogue(2);
                    OpenDialogue(0);
                    break;

                case "Stage003":
                    SetDialogue(3);
                    break;

                case "Stage004":
                    SetDialogue(4);
                    break;

                case "Stage015":
                    SetDialogue(5);
                    Debug.Log("SwitchCurrentStage");
                    Animator boss = FindAnyObjectByType<LastBossManager>().GetComponent<Animator>();
                    if (SaveLoadManager.Instance.scenarioSaveData.flynneEvent[5])
                    {
                        if (!OpenDialogue(1))
                            boss.SetBool("Start", true);
                    }
                    else
                    {
                        if (!OpenDialogue(0))
                        {
                            Debug.Log("Boss Start");
                            boss.SetBool("Start", true);
                        }
                    }
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
            InitializeManager.Instance.Set_Second += Initialize;
        }

        private void Initialize()
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
            miniMap = FindAnyObjectByType<Mini_Map>().gameObject;

            StageManager.Instance.stageEndEvent += ReadyComposition;
            StageManager.Instance.stageEndEvent += InvokeStage;

            // 대화 이벤트 관리
            uiManager = DataManager.Instance.UIManager;
            uiManager.dialogueUI.OnDialogueEnter += OnDialogue;
            uiManager.dialogueUI.OnDialogueEnd += OffDialogue;

            CurrentDialogue = dialogueList[currentIndex];
        }

        private void OnDialogue() => IsDialogue = true;

        private void OffDialogue() => IsDialogue = false;

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