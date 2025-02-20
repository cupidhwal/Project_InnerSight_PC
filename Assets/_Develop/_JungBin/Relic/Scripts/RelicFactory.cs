using System.Collections.Generic;
using UnityEngine;

namespace JungBin
{

    public static class RelicFactory
    {
        // 🔹 하나의 Dictionary만 유지 (영어 ID → 한글 이름)
        private static Dictionary<string, string> relicData = new Dictionary<string, string>()
    {
        { "ResurrectionRing", "생명의 고리" },
    };

        // 🔹 유물 ID로 객체 생성
        public static IRelic CreateRelic(string relicID)
        {
            switch (relicID)
            {
                case "ResurrectionRing": return new ResurrectionRelic();
                default:
                    Debug.LogWarning($"알 수 없는 유물 ID: {relicID}");
                    return null;
            }
        }

        // 🔹 영어 ID → 한글 이름 변환
        public static string GetRelicName(string relicID)
        {
            return relicData.ContainsKey(relicID) ? relicData[relicID] : "알 수 없는 유물";
        }

        // 🔹 한글 이름 → 영어 ID 변환 (역변환)
        public static string GetRelicID(string relicName)
        {
            foreach (var pair in relicData)
            {
                if (pair.Value == relicName)
                {
                    return pair.Key;
                }
            }
            return null;
        }
    }
}