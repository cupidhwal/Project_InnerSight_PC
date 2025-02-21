using Noah;
using System;
using System.Collections.Generic;

namespace JungBin
{
    /// <summary>
    /// 유물 데이터 저장을 위한 구조체
    /// </summary>
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

    /// <summary>
    /// 유물 저장 데이터를 관리하는 클래스
    /// </summary>
    [Serializable]
    public class RelicSaveData
    {
        public List<RelicDataEntry> relics = new List<RelicDataEntry>(); // 🔹 유물 리스트 (ID + Name)

        /// <summary>
        /// 저장된 유물 데이터를 초기화
        /// </summary>
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