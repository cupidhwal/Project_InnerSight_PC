using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public static class SkillPool<T> where T : Skill<T>
    {
        private static Queue<T> pool = new Queue<T>();

        // Ǯ���� ��ų ��������
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

        // ��ų�� Ǯ�� ��ȯ
        public static void ReturnObject(T obj)
        { 
            pool.Enqueue(obj);
        }
    }
}