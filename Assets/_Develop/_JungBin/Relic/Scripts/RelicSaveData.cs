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
            if (!SaveLoadManager.Instance.IsLoadData(SaveLoadManager.Instance.relicSavePath))
            {
                relics.Clear(); // 🔹 저장된 데이터가 없으면 리스트를 비워서 오류 방지
            }
            else
            {
                // 저장된 데이터를 불러옴
                RelicSaveData loadedRelic = SaveLoadManager.Instance.relicSaveData;
                relics.Clear(); // 🔹 기존 데이터를 초기화하여 중복 저장 방지

                // 🔹 오류 방지를 위해 Add()를 사용하여 리스트에 새 데이터 추가
                for (int i = 0; i < loadedRelic.relics.Count; i++)
                {
                    relics.Add(new RelicDataEntry(loadedRelic.relics[i].relicID, loadedRelic.relics[i].relicName));
                }
            }
        }

    }


}