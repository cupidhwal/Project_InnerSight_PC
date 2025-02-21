using Noah;
using System;
using System.Collections.Generic;

namespace JungBin
{

    [Serializable]
    public class RelicDataEntry
    {
        public string relicID;   // 영어 ID
        public string relicName; // 한글 이름

        public RelicDataEntry(string id, string name)
        {
            relicID = id;
            relicName = name;
        }


    }

    [Serializable]
    public class RelicSaveData
    {
        public List<RelicDataEntry> relics = new List<RelicDataEntry>(); // 🔹 유물 리스트 (ID + Name)

        public void ResetData()
        {
            if (!SaveLoadManager.Instance.isLoadData)
            {
                relics[0].relicID = "";
                relics[0].relicName = "";
            }
            else
            {
                // 저장된 데이터가 있으면 적용
                RelicSaveData loadedRelic = SaveLoadManager.Instance.relicSaveData;

                for (int i = 0; i < loadedRelic.relics.Count; i++)
                {
                    relics[i].relicID = loadedRelic.relics[i].relicID;
                    relics[i].relicName = loadedRelic.relics[i].relicName;
                }
            }
        }
    }


}