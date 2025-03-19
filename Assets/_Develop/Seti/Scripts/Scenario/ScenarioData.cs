using Noah;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    [Serializable]
    public class ScenarioProgress
    {
        public string ScenarioName = string.Empty;
        public bool[] CheckSeens;
        public bool SeenCompleted = false;

        public ScenarioProgress(DialogueData data)
        {
            ScenarioName = data.name;
            CheckSeens = data.GetSeenList();
            SeenCompleted = data.SeenCompleted;
        }

        public void SetData(DialogueData data)
        {
            ScenarioName = data.name;
            CheckSeens = data.GetSeenList();
            SeenCompleted = data.SeenCompleted;
        }
    }

    [Serializable]
    public class ScenarioData
    {
        public int deathCount;
        public bool[] sinEvent = new bool[6];
        public bool[] flynneEvent = new bool[6];

        public List<ScenarioProgress> dialogueDatas = new();

        public void ResetData()
        {
            SaveLoadManager save = SaveLoadManager.Instance;
            if (save.IsLoadData(save.scenarioSaveDataPath))
            {
                ScenarioData scenarioData = save.scenarioSaveData;

                for (int i = 0; i < dialogueDatas.Count; i++)
                {
                    dialogueDatas[i].ScenarioName = scenarioData.dialogueDatas[i].ScenarioName;
                    dialogueDatas[i].CheckSeens = scenarioData.dialogueDatas[i].CheckSeens;
                    dialogueDatas[i].SeenCompleted = scenarioData.dialogueDatas[i].SeenCompleted;
                }

                save.CheckTutorial(save.scenarioSaveData.dialogueDatas[0].SeenCompleted);
            }
        }
    }
}