using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 게임에서 사용하는 데이터들을 관리하는 클래스
    /// </summary>
    public class DataManager : Singleton<DataManager>
    {
        // 필드
        #region Variables
        [Header("Data : Dialogue")]
        public List<DialogueData> dialogueDatas = new();

        private DialogueData dialogueData = null;
        private EffectData effectData = null;
        private QuestData questData = null;
        #endregion

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

        //이펙트 데이터 가져오기
        public DialogueData GetDialogData()
        {
            string dataName = string.Empty;
            switch (StoryManager.Instance.StageName)
            {
                case "_T":
                    dataName = "_T";
                    break;

                case "00":
                    dataName = "00";
                    break;

                case "01":
                    dataName = "01";
                    break;
            }
            dataName = "Stage" + dataName;

            for (int i = 0; i < dialogueDatas.Count; i++)
            {
                if (dialogueDatas[i].name == dataName)
                    return dialogueDatas[i];
            }

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
    }
}