using System;
using System.Collections.Generic;

namespace Seti
{
    // LINQ의 메서드를 재현하는 유틸리티
    public static class CollectionUtility
    {
        // T의 첫 번째 요소를 찾는 메서드
        public static T First<T>(IEnumerable<T> source)
        {
            foreach (T element in source)
            {
                return element;
            }
            return default;
        }

        // FirstOrDefault 유틸리티 메서드 구현
        public static T FirstOrDefault<T>(IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (T element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }
            return default;
        }

        public static T FirstOrNull<T>(IEnumerable<T> source, Func<T, bool> predicate) where T : class
        {
            foreach (T element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }
            return null;
        }

        // 리스트의 인덱스를 바꾸는 메서드
        public static void SwapListElements<T>(List<T> list, int indexA, int indexB)
        {
            (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
        }

    }
}