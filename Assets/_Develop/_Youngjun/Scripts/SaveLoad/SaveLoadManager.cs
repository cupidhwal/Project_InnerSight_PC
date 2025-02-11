using Noah;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public string playerStatsSavePath = "/PlayerStats.json";
    public PlayerData playerStats = new PlayerData();

    public string upgradeCountSavePath = "/UpGradeCountData.json";
    public UpGradeCountData upgradeCount = new UpGradeCountData();

    public string playerItemSavePath = "/PlayerItem.json";
    public PlayerItem playerItem = new PlayerItem();

    //public string upgradeGoldSavePath = "/UpgradeGold.json";
    //public Gold upgradeGold = new Gold();

    public bool isLoadData;

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    void Init()
    {
        //playerStats = new PlayerData(ps_Manager.startPlayerData.hp_Start, ps_Manager.startPlayerData.atk_Start,
        //    ps_Manager.startPlayerData.def_Start, ps_Manager.startPlayerData.moveSpeed_Start, ps_Manager.startPlayerData.atkSpeed_Start);

        LoadAll();
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

            isLoadData = true;
        }
        else
        {
            isLoadData = false;
            Debug.Log("세이브 데이터가 없습니다");
        }
    }
    public void LoadAll()
    {
        LoadData(playerStatsSavePath, ref playerStats);
        LoadData(upgradeCountSavePath, ref upgradeCount);
        LoadData(playerItemSavePath, ref playerItem);
        //LoadData(upgradeGoldSavePath, ref upgradeGold);
    }

    // 데이터 삭제
    [ContextMenu("Clear")]
    public void DeleteAllSaveFiles()
    {
        string saveDirectory = Application.persistentDataPath;

        if (Directory.Exists(saveDirectory))
        {
            string[] files = Directory.GetFiles(saveDirectory);

            foreach (string file in files)
            {
                File.Delete(file);
                Debug.Log("Deleted: " + file);
            }
        }
        else
        {
            Debug.LogWarning("Save directory not found!");
        }
    }

}
