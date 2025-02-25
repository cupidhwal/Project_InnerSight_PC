using System.Collections.Generic;
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
        [Header("Data : Dialogue")]
        public List<DialogueData> dialogueDatas;

        private DialogueData dialogueData = null;
        private EffectData effectData = null;
        private QuestData questData = null;
        #endregion

        // 속성
        public DialogueData DialogueData => dialogueData;

        private void Start()
        {
            /*//대화 데이터 가져오기
            if (dialogueData == null)
            {
                dialogueData = ScriptableObject.CreateInstance<DialogueData>();
                dialogueData.LoadData();
            }

            //이펙트 데이터 가져오기
            if (effectData == null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }

            //퀘스트 데이터 가져오기
            if (questData == null)
            {
                questData = ScriptableObject.CreateInstance<QuestData>();
                questData.LoadData();
            }*/
        }

        protected override void Awake()
        {
            base.Awake();
        }

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