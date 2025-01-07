using UnityEngine;

namespace Seti
{
    public static class LetterUtility
    {
        public static bool HasFinalConsonant(char ch)
        {
            // 한글 유니코드 범위 내에 있는지 확인 (0xAC00 ~ 0xD7A3)
            if (ch < 0xAC00 || ch > 0xD7A3)
            {
                return false; // 한글이 아니면 받침 없음으로 간주
            }

            // 한글 유니코드 기준 '가'를 뺀 값으로 계산
            int unicodeValue = ch - 0xAC00;
            int jongseongIndex = unicodeValue % 28; // 28로 나눈 나머지가 종성 (받침) 인덱스

            return jongseongIndex != 0; // 0이면 받침이 없는 경우
        }

        public static string GetLastString(string foodName)
        {
            char lastChar = foodName[^1];
            string ending = HasFinalConsonant(lastChar) ? "이야" : "야";
            return $"{foodName}{ending}";
        }
    }
}