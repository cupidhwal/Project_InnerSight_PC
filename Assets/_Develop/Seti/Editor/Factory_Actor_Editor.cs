using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Seti
{
    [CustomEditor(typeof(Factory_Actor))]
    public class Factory_Actor_Editor : Editor
    {
        // 필드
        #region Variables
        private Factory_Actor factory;
        public ReorderableList SpawnedActorsList => spawnedActorsList;
        private ReorderableList spawnedActorsList;      // 생성된 액터 리스트
        private ReorderableList selectedBlueprintsList; // 선택된 Blueprint 리스트

        private int selectedIndex = 0; // 드롭다운에서 선택된 Blueprint 인덱스
        private readonly List<Blueprint_Actor> selectedBlueprints = new(); // 선택된 Blueprint 저장
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void OnEnable()
        {
            factory = (Factory_Actor)target;

            InitializeBlueprintsList();
            InitializeSpawnedActorsList();
        }
        #endregion

        // 메서드
        #region Methods
        public override void OnInspectorGUI()
        {
            EditUtility.SubjectLine(2, "Actor Factory");

            DrawDefaultInspector();

            if (factory == null)
            {
                EditorGUILayout.HelpBox("Factory_Actor 인스턴스가 초기화되지 않았습니다.", MessageType.Error);
                return;
            }

            DrawBlueprintManagementUI();
            DrawActorManagementUI();

            // Delete 키 이벤트 감지
            HandleDeleteKey();

            EditUtility.DrawLine(2);
        }

        /// <summary>
        /// ReorderableList의 Actor 리스트 갱신
        /// </summary>
        public void UpdateSpawnedActorsList()
        {
            spawnedActorsList.list = factory.MadeObjects.ToList();
            Repaint();
        }

        /// <summary>
        /// Blueprint 관리 UI
        /// </summary>
        private void DrawBlueprintManagementUI()
        {
            if (GUILayout.Button("Blueprints 갱신"))
            {
                RefreshBlueprints(factory);
                Debug.Log($"Blueprint 갱신: 총 {factory.blueprints.Count}개의 Blueprint를 찾았습니다.");
            }

            EditUtility.SubjectLine(Color.gray, 2, "Actor 생성");

            if (factory.blueprints != null && factory.blueprints.Count > 0)
            {
                string[] blueprintNames = factory.blueprints.Select(bp => bp.ActorName).ToArray();
                selectedIndex = EditorGUILayout.Popup("생성할 Actor 선택", selectedIndex, blueprintNames);

                if (GUILayout.Button("선택한 Actors에 추가"))
                {
                    AddSelectedActorToList(factory, selectedIndex);
                }

                selectedBlueprintsList.DoLayoutList();

                if (GUILayout.Button("Scene에 선택 Actor 배치"))
                {
                    PlaceSelectedActors();
                }

                EditUtility.DrawLine(Color.gray, 1);

                if (GUILayout.Button("Scene에 모든 Actor 배치"))
                {
                    PlaceAllActors(factory);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Blueprints가 비어 있습니다. 갱신 버튼을 눌러 업데이트하세요.", MessageType.Warning);
            }
        }

        /// <summary>
        /// Actor 관리 UI
        /// </summary>
        private void DrawActorManagementUI()
        {
            EditUtility.SubjectLine(Color.gray, 2, "Actor 관리");

            UpdateSpawnedActorsList();
            spawnedActorsList.DoLayoutList();

            // 선택된 Actor 설계도 갱신
            if (GUILayout.Button("선택 Actor 갱신"))
            {
                int selectedIndex = spawnedActorsList.index;
                if (selectedIndex >= 0 && selectedIndex < factory.MadeObjects.Count)
                {
                    RefreshBlueprints(factory);
                    factory.ApplyBlueprintToActor(selectedIndex);
                }
                else
                {
                    Debug.LogWarning("유효하지 않은 Actor가 선택되었습니다.");
                }
            }

            GUILayout.Space(2.5f);

            // 모든 Actor 설계도 갱신
            if (GUILayout.Button("모든 Actor 갱신"))
            {
                factory.ApplyBlueprintToAllActors();
                UpdateSpawnedActorsList();
                Repaint();
            }

            EditUtility.DrawLine(Color.gray, 1);

            // 선택된 Actor 제거
            if (GUILayout.Button("선택 Actor 제거"))
            {
                int selectedIndex = spawnedActorsList.index; // 선택된 인덱스 가져오기

                if (selectedIndex >= 0 && selectedIndex < factory.MadeObjects.Count)
                {
                    GameObject selectedActor = factory.MadeObjects[selectedIndex];
                    factory.DestroyActor(selectedActor); // 선택된 액터 제거
                    UpdateSpawnedActorsList();
                    Repaint();
                }
                else
                {
                    Debug.LogWarning("선택된 Actor가 없습니다!");
                }
            }

            GUILayout.Space(2.5f);

            // 모든 Actor 제거
            if (GUILayout.Button("모든 Actor 제거"))
            {
                factory.DestroyAllActors();
                UpdateSpawnedActorsList();
                Repaint();
            }
        }

        /// <summary>
        /// 선택된 Blueprint를 관리하는 ReorderableList 초기화
        /// </summary>
        private void InitializeBlueprintsList()
        {
            selectedBlueprintsList = new ReorderableList(selectedBlueprints, typeof(Blueprint_Actor), true, true, false, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "선택한 Actors"),
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    if (index < selectedBlueprints.Count)
                    {
                        var blueprint = selectedBlueprints[index];
                        EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width - 30, EditorGUIUtility.singleLineHeight), blueprint.ActorName);

                        if (GUI.Button(new Rect(rect.x + rect.width - 25, rect.y, 25, EditorGUIUtility.singleLineHeight), "-"))
                        {
                            selectedBlueprints.RemoveAt(index);
                        }
                    }
                }
            };
        }

        /// <summary>
        /// 생성된 Actor를 관리하는 ReorderableList 초기화
        /// </summary>
        private void InitializeSpawnedActorsList()
        {
            spawnedActorsList = new ReorderableList(factory.MadeObjects.ToList(), typeof(GameObject), false, true, false, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "생성된 Actors"),
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var actors = factory.MadeObjects;
                    if (index < actors.Count && actors[index] != null)
                    {
                        EditorGUI.LabelField(rect, actors[index].name);
                    }
                    else
                    {
                        EditorGUI.LabelField(rect, "<NULL>");
                    }
                },
                // 리스트 항목 강조 표시 (하이어라키만 갱신)
                onSelectCallback = list =>
                {
                    var actors = factory.MadeObjects;
                    if (list.index >= 0 && list.index < actors.Count && actors[list.index] != null)
                    {
                        EditorGUIUtility.PingObject(actors[list.index]); // 하이어라키 강조
                    }
                }
            };
        }

        /// <summary>
        /// 선택된 Blueprint를 선택한 목록에 추가
        /// </summary>
        private void AddSelectedActorToList(Factory_Actor factory, int index)
        {
            if (index < 0 || index >= factory.blueprints.Count)
            {
                Debug.LogWarning("유효하지 않은 Blueprint 선택입니다.");
                return;
            }

            selectedBlueprints.Add(factory.blueprints[index]);
        }

        /// <summary>
        /// 선택된 Actor 목록을 씬에 배치
        /// </summary>
        private void PlaceSelectedActors()
        {
            foreach (var blueprint in selectedBlueprints)
            {
                factory.CreateActor(blueprint);
            }
            UpdateSpawnedActorsList();
            Repaint();
        }

        /// <summary>
        /// 모든 Blueprint를 기반으로 Actor를 씬에 배치
        /// </summary>
        private void PlaceAllActors(Factory_Actor factory)
        {
            foreach (var blueprint in factory.blueprints)
            {
                factory.CreateActor(blueprint);
            }
            UpdateSpawnedActorsList();
            Repaint();
        }

        /// <summary>
        /// 프로젝트 내의 모든 Blueprint를 Factory_Actor에 업데이트
        /// </summary>
        private void RefreshBlueprints(Factory_Actor factory)
        {
            string[] guids = AssetDatabase.FindAssets("t:Blueprint_Actor");
            var allBlueprints = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<Blueprint_Actor>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(blueprint => blueprint != null)
                .ToList();

            // 플레이어를 0번 인덱스로 정렬
            factory.blueprints = allBlueprints.OrderBy(bp => bp.ActorName == "Player" ? 0 : 1).ToList();

            // 변경 사항 강제 저장
            EditorUtility.SetDirty(factory);
        }

        /// <summary>
        /// ReorderableList의 Actor 제거 - Delete 키 입력
        /// </summary>
        private void HandleDeleteKey()
        {
            Event e = Event.current; // 현재 이벤트 가져오기
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
            {
                // ReorderableList에서 선택된 항목 인덱스 가져오기
                int selectedIndex = spawnedActorsList.index;

                if (selectedIndex >= 0 && selectedIndex < factory.MadeObjects.Count)
                {
                    // 선택된 Actor 제거
                    GameObject selectedActor = factory.MadeObjects[selectedIndex];
                    factory.DestroyActor(selectedActor);
                    UpdateSpawnedActorsList();
                    Repaint();
                }
                else
                {
                    Debug.LogWarning("선택된 Actor가 없습니다.");
                }

                e.Use(); // 이벤트 소비 (다른 곳에서 처리되지 않도록)
            }
        }
        #endregion
    }
}