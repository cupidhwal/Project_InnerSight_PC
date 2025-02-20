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
    }
}