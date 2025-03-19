using JungBin;
using Seti;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Noah
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private Dictionary<string, bool> dataGroupDic = new Dictionary<string, bool>();

        public string playerStatsSavePath = "/PlayerStats.json";
        public PlayerData playerStats = new PlayerData();

        public string upgradeCountSavePath = "/UpGradeCountData.json";
        public UpGradeCountData upgradeCount = new UpGradeCountData();

        public string playerItemSavePath = "/PlayerItem.json";
        public PlayerItem playerItem = new PlayerItem();

        public string relicSavePath = "/RelicData.json";
        public RelicSaveData relicSaveData = new RelicSaveData();

        public string scenarioSaveDataPath = "/scenarioData.json";
        public ScenarioData scenarioSaveData = new();

        //public string upgradeGoldSavePath = "/UpgradeGold.json";
        //public Gold upgradeGold = new Gold();

        public bool isTutorial;

        protected override void Awake()
        {
            base.Awake();

            if (SceneManager.GetActiveScene().name != "MainMenu") // 타이틀 씬이 아닐 때만 Init 호출
            {
                Init();
            }
            else
            {
                LoadAll();
            }
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name != "MainMenu") // 타이틀 씬이 아닐 때만 Init 호출
            {
                DataReset();

                PlayerStatsManager.Instance.Init();
            }

      
        }

        void Init()
        {
            AddDictionary();

            LoadAll();

            scenarioSaveData.ResetData();
        }

        void DataReset()
        {
            playerStats.ResetData();
            playerItem.ResetData();
            relicSaveData.ResetData();
        }

        void AddDictionary()
        {
            dataGroupDic.Add(playerStatsSavePath, false);
            dataGroupDic.Add(upgradeCountSavePath, false);
            dataGroupDic.Add(playerItemSavePath, false);
            dataGroupDic.Add(relicSavePath, false);
            dataGroupDic.Add(scenarioSaveDataPath, false);
        }

        public bool IsLoadData(string _path)
        {
            return dataGroupDic[_path];
        }

        [ContextMenu("Save")]
        public void Save<T>(string _path, T _data)
        {
            string path = Application.persistentDataPath + _path;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);

            // 데이터를 JSON으로 변환
            string saveData = JsonUtility.ToJson(_data, true);
            bf.Serialize(fs, saveData);
            fs.Close();

            Debug.Log("Saved: " + path);
            Debug.Log(saveData);
        }

        public void SaveAll()
        {
            Save(playerStatsSavePath, playerStats);
            Save(upgradeCountSavePath, upgradeCount);
            Save(playerItemSavePath, playerItem);
            // Save(upgradeGoldSavePath, upgradeGold);
        }

        [ContextMenu("Load")]
        public void LoadData<T>(string _path, ref T _container)
        {
            string path = Application.persistentDataPath + _path;

            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

                string loadData = bf.Deserialize(fs).ToString();
                JsonUtility.FromJsonOverwrite(loadData, _container);
                fs.Close();

                Debug.Log("Loaded: " + loadData);

                dataGroupDic[_path] = true;
            }
            else
            {
                dataGroupDic[_path] = false;

                if (dataGroupDic.TryGetValue(_path, out bool value))
                {
                    Debug.Log($"{_path} Value: {value}");
                }
                else
                {
                    Debug.Log("Key not found!");
                }

                Debug.Log("세이브 데이터가 없습니다");     
            }
        }

        public void EmptyData(GameObject _object)
        {
            if (dataGroupDic.TryGetValue(playerStatsSavePath, out bool value))
            {
                if (!value)
                {
                    _object.SetActive(false);
                }
                else
                {
                    _object.SetActive(true);
                }

                Debug.Log($"{playerStatsSavePath} Value: {value}");
            }
            else
            {
                Debug.Log("Key not found!");
            }

        }

        public void LoadAll()
        {
            LoadData(playerStatsSavePath, ref playerStats);
            LoadData(upgradeCountSavePath, ref upgradeCount);
            LoadData(playerItemSavePath, ref playerItem);
            LoadData(relicSavePath, ref relicSaveData);
            LoadData(scenarioSaveDataPath, ref scenarioSaveData);

            //LoadData(upgradeGoldSavePath, ref upgradeGold);
        }

        // 데이터 삭제
        [ContextMenu("Clear")]
        public void DeleteAllSaveFiles()
        {
            string saveDirectory = Application.persistentDataPath;
            Debug.Log("Save Directory: " + saveDirectory);

            if (Directory.Exists(saveDirectory))
            {
                string[] files = Directory.GetFiles(saveDirectory);

                foreach (string file in files)
                {
                    // Player.log 파일은 삭제하지 않도록 예외 처리
                    if (file.EndsWith("Player.log"))
                    {
                        Debug.LogWarning("Skipping Player.log file.");
                        continue;
                    }

                    try
                    {
                        File.Delete(file);
                        Debug.Log("Deleted: " + file);
                    }
                    catch (IOException e)
                    {
                        Debug.LogError("Failed to delete file: " + file + "\nError: " + e.Message);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Save directory not found!");
            }
        }

        public void SaveRelics()
        {
            relicSaveData.relics.Clear();
            foreach (var relic in RelicManager.Instance.GetRelics())
            {
                relicSaveData.relics.Add(new RelicDataEntry(relic.RelicID, relic.RelicName));
            }
            Save(relicSavePath, relicSaveData);
        }

        public void SaveScenario(DialogueData dialogueData)
        {
            scenarioSaveData.deathCount = DataManager.Instance.deathCount;
            scenarioSaveData.sinEvent = DataManager.Instance.sinEvent;
            scenarioSaveData.flynneEvent = DataManager.Instance.flynneEvent;
            ScenarioProgress data = scenarioSaveData.dialogueDatas.FirstOrDefault(dialogue => dialogue.ScenarioName == dialogueData.name);
            if (data == null)
            {
                data = new(dialogueData);
                scenarioSaveData.dialogueDatas.Add(data);
            }
            data.SetData(dialogueData);

            // 첫 번째 원흉 이벤트 체크
            if (!scenarioSaveData.sinEvent[0] && data.ScenarioName == "Stage001" && data.CheckSeens[2])
            {
                data.CheckSeens[2] = false;
            }

            // 두 번째 원흉 이벤트 체크
            if (!scenarioSaveData.sinEvent[1] && data.ScenarioName == "Stage004" && data.CheckSeens[0])
            {
                data.CheckSeens[0] = false;
            }

            Save(scenarioSaveDataPath, scenarioSaveData);
        }
        public void CheckTutorial(bool flag) => isTutorial = flag;
    }
}

