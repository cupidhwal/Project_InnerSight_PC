using System.Collections.Generic;
using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// 유물을 ID를 기반으로 생성하는 팩토리
    /// </summary>
    public static class RelicFactory
    {
        // 🔹 하나의 Dictionary만 유지 (영어 ID → 한글 이름)
        private static Dictionary<string, string> relicData = new Dictionary<string, string>()
        {
            { "ResurrectionRing", "생명의 고리" },
        };

        /// <summary>
        /// 유물 ID로 객체 생성
        /// </summary>
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

        /// <summary>
        /// 영어 ID → 한글 이름 변환
        /// </summary>
        public static string GetRelicName(string relicID)
        {
            return relicData.ContainsKey(relicID) ? relicData[relicID] : "알 수 없는 유물";
        }

        /// <summary>
        /// 한글 이름 → 영어 ID 변환 (역변환)
        /// </summary>
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
