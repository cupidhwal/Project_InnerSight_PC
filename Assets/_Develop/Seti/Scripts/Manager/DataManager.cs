using Noah;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 게임에서 사용하는 데이터들을 관리하는 클래스
    /// </summary>
    public class DataManager : PersistentSingleton<DataManager>
    {
        // 필드
        #region Variables
        [Header("Data : Player")]
        public int deathCount;
        public bool[] sinEvent = new bool[6];

        [Header("Data : Dialogue")]
        public List<DialogueData> dialogueDatas;

        private DialogueData dialogueData = null;
        private EffectData effectData = null;
        private QuestData questData = null;
        #endregion

        // 속성
        public DialogueData DialogueData => dialogueData;
        public UIManager UIManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            InitializeManager.Instance.Set_First += Initialize;
        }

        // 초기화
        void Initialize() => UIManager = GetComponent<UIManager>();

        // 대화 데이터 가져오기
        public DialogueData GetDialogData()
        {
            string dataName = StoryManager.Instance.StageName;

            dialogueDatas ??= new();

            // 기존 대화 데이터 검색
            for (int i = 0; i < dialogueDatas.Count; i++)
            {
                if (dialogueDatas[i].name == dataName)
                {
                    dialogueData = dialogueDatas[i];
                    return dialogueData;
                }
            }

            // 새로운 대화 데이터 생성
            dialogueData = ScriptableObject.CreateInstance<DialogueData>();
            dialogueData.LoadData();
            dialogueData.name = dataName;

            // 세이브 데이터 읽어오기
            ScenarioData data = SaveLoadManager.Instance.scenarioSaveData;
            if (data != null)
            {
                ScenarioProgress progress = data.dialogueDatas.FirstOrDefault(dialogue => dialogue.ScenarioName == dataName);
                if (progress != null)
                {
                    dialogueData.CheckSeens = progress.CheckSeens;
                    dialogueData.SeenCompleted = progress.SeenCompleted;
                }
                deathCount = data.deathCount;
                sinEvent = data.sinEvent;
    }
            dialogueDatas.Add(dialogueData);

            return dialogueData;
        }

        // 이펙트 데이터 가져오기
        public EffectData GetEffectData()
        {
            if (effectData == null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }
            return effectData;
        }

        // 퀘스트 데이터 가져오기
        public QuestData GetQuestData()
        {
            if (questData == null)
            {
                questData = ScriptableObject.CreateInstance<QuestData>();
                questData.LoadData();
            }
            return questData;
        }

        private void OnValidate()
        {
            dialogueDatas.Sort();
        }
    }
}