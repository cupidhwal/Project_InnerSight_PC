using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Factory의 유일성을 보장하기 위해 싱글톤 패턴으로 설계
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Factory<T> : ScriptableObject where T : Factory<T>
    {
        // 싱글톤
        #region Singleton
        private static T instance;

        /// <summary>
        /// 유일한 Factory 인스턴스
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // Resources 폴더에서 해당 Factory 타입의 에셋 검색
                    instance = Resources.Load<T>(typeof(T).Name);

                    if (instance == null)
                    {
                        Debug.LogError($"Factory<{typeof(T).Name}> 인스턴스를 찾을 수 없습니다. " +
                                       $"Resources 폴더에 {typeof(T).Name} 에셋을 배치하세요.");
                    }
                }
                return instance;
            }
        }
        #endregion

        // 필드
        #region Variables
        [HideInInspector]
        [Tooltip("생성한 GameObject를 저장하는 리스트")]
        public List<GameObject> madeObjects = new();
        /// <summary>
        /// 생성된 액터를 읽기 전용으로 반환
        /// </summary>
        public IReadOnlyList<GameObject> MadeObjects => madeObjects;
        #endregion

        /// <summary>
        /// 초기화 로직 (필요시 오버라이드)
        /// </summary>
        public virtual void Initialize()
        {
            Debug.Log($"{typeof(T).Name} 초기화 완료");
        }

        /*[InitializeOnLoadMethod]
        private static void EnsureUniqueInstance()
        {
            // 프로젝트 내의 모든 Factory_Actor 에셋 검색
            var allInstances = AssetDatabase.FindAssets("t:Factory_Actor");

            if (allInstances.Length > 1)
            {
                Debug.LogWarning("Factory_Actor는 프로젝트 내에 하나만 존재해야 합니다!");
                foreach (var guid in allInstances)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    Debug.Log($"Factory_Actor 에셋 경로: {path}");
                }
            }
        }*/
    }

    /*public static class FactoryMenu
    {
        [MenuItem("Assets/Create/Factory/Actor", true)] // 조건부 활성화
        private static bool ValidateCreateFactoryActor()
        {
            // 유일성 검사: 프로젝트 내에 Factory_Actor가 이미 존재하면 비활성화
            var existingFactories = AssetDatabase.FindAssets("t:Factory_Actor");
            return existingFactories.Length == 0;
        }

        [MenuItem("Assets/Create/Factory/Actor")]
        private static void CreateFactoryActor()
        {
            // 팩토리 생성
            var factory = ScriptableObject.CreateInstance<Factory_Actor>();

            // Resources 폴더에 에셋 저장
            var path = "Assets/Resources/Factory_Actor.asset";
            AssetDatabase.CreateAsset(factory, path);
            AssetDatabase.SaveAssets();

            Debug.Log("Factory_Actor 에셋이 생성되었습니다.");
        }
    }*/
}