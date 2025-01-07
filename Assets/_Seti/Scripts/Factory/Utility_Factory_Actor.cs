using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Seti
{
    [InitializeOnLoad]
    public static class Utility_Factory_Actor
    {
        static Utility_Factory_Actor()
        {
            // 하이어라키 변경 및 씬 로드 시점에 동기화
            EditorApplication.hierarchyChanged += SyncAllFactories;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            EditorSceneManager.sceneLoaded += (scene, mode) => SyncAllFactories();

            // SceneView 이벤트에 구독
            //SceneView.duringSceneGui += OnSceneGUI;
        }

        /// <summary>
        /// 모든 Factory_Actor 인스턴스를 동기화
        /// </summary>
        private static void SyncAllFactories()
        {
            // Factory_Actor 인스턴스 찾기
            var factoryInstances = Resources.FindObjectsOfTypeAll<Factory_Actor>();
            foreach (var factory in factoryInstances)
            {
                SyncFactoryWithScene(factory);
            }

            // 에디터의 모든 인스펙터 창 갱신
            RepaintAllInspectors();
        }

        /// <summary>
        /// 특정 Factory_Actor 인스턴스를 현재 씬의 액터와 동기화
        /// </summary>
        private static void SyncFactoryWithScene(Factory_Actor factory)
        {
            // 현재 씬의 모든 Actor 찾기
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            var actorsInScene = Object.FindObjectsOfType<Actor>();
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

            // 팩토리의 SpawnedActors 리스트를 씬의 Actor와 동기화
            factory.madeObjects.Clear();
            foreach (var actor in actorsInScene)
            {
                if (!factory.madeObjects.Contains(actor.gameObject))
                {
                    factory.madeObjects.Add(actor.gameObject);
                }
            }

            // 팩토리 데이터 저장
            EditorUtility.SetDirty(factory);
        }

        /// <summary>
        /// ReorderableList의 Actor를 Hierarchy와 동기화
        /// </summary>
        private static void OnHierarchyChanged()
        {
            var factoryInstances = Resources.FindObjectsOfTypeAll<Factory_Actor>();

            foreach (var factory in factoryInstances)
            {
                // 하이어라키 순서로 동기화
                factory.madeObjects = factory.madeObjects
                    .OrderBy(actor => actor.transform.GetSiblingIndex())
                    .ToList();

                EditorUtility.SetDirty(factory);
            }

            RepaintAllInspectors();
        }

        /// <summary>
        /// 에디터의 모든 인스펙터 창 갱신
        /// </summary>
        private static void RepaintAllInspectors()
        {
            foreach (var inspector in Resources.FindObjectsOfTypeAll<EditorWindow>())
            {
                if (inspector.GetType().Name == "InspectorWindow")
                {
                    inspector.Repaint();
                }
            }
        }

        /// <summary>
        /// ReorderableList의 Actor를 키 입력으로 제거 - 아직 작동 안 됨
        /// </summary>
        /// <param name="sceneView"></param>
        /*private static void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
            {
                // 현재 선택된 팩토리 오브젝트 가져오기
                var factory = Selection.activeObject as Factory_Actor;

                if (factory != null)
                {
                    // 선택된 ReorderableList의 항목 인덱스 가져오기
                    var editor = Editor.CreateEditor(factory) as Factory_Actor_Editor;
                    if (editor != null && editor.SpawnedActorsList.index >= 0)
                    {
                        int selectedIndex = editor.SpawnedActorsList.index;
                        if (selectedIndex >= 0 && selectedIndex < factory.MadeObjects.Count)
                        {
                            GameObject selectedActor = factory.MadeObjects[selectedIndex];
                            factory.DestroyActor(selectedActor);
                            editor.UpdateSpawnedActorsList();
                            editor.Repaint();

                            Debug.Log($"'{selectedActor.name}'가 삭제되었습니다.");
                        }
                    }
                }

                e.Use(); // 이벤트 소비
            }
        }*/
    }
}