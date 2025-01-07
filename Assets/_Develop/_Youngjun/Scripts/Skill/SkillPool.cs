using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public static class SkillPool<T> where T : Skill<T>
    {
        private static Queue<T> pool = new Queue<T>();

        // 풀에서 스킬 가져오기
        public static T GetObject(System.Func<T> factoryMethod)
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            else 
            {
                return factoryMethod();
            }
            
        }

        // 스킬을 풀에 반환
        public static void ReturnObject(T obj)
        { 
            pool.Enqueue(obj);
        }
    }
}