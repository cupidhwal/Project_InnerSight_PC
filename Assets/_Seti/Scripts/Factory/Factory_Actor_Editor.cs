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
        // �ʵ�
        #region Variables
        private Factory_Actor factory;
        public ReorderableList SpawnedActorsList => spawnedActorsList;
        private ReorderableList spawnedActorsList;      // ������ ���� ����Ʈ
        private ReorderableList selectedBlueprintsList; // ���õ� Blueprint ����Ʈ

        private int selectedIndex = 0; // ��Ӵٿ�� ���õ� Blueprint �ε���
        private readonly List<Blueprint_Actor> selectedBlueprints = new(); // ���õ� Blueprint ����
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void OnEnable()
        {
            factory = (Factory_Actor)target;

            InitializeBlueprintsList();
            InitializeSpawnedActorsList();
        }
        #endregion

        // �޼���
        #region Methods
        public override void OnInspectorGUI()
        {
            EditUtility.SubjectLine(2, "Actor Factory");

            DrawDefaultInspector();

            if (factory == null)
            {
                EditorGUILayout.HelpBox("Factory_Actor �ν��Ͻ��� �ʱ�ȭ���� �ʾҽ��ϴ�.", MessageType.Error);
                return;
            }

            DrawBlueprintManagementUI();
            DrawActorManagementUI();

            // Delete Ű �̺�Ʈ ����
            HandleDeleteKey();

            EditUtility.DrawLine(2);
        }

        /// <summary>
        /// ReorderableList�� Actor ����Ʈ ����
        /// </summary>
        public void UpdateSpawnedActorsList()
        {
            spawnedActorsList.list = factory.MadeObjects.ToList();
            Repaint();
        }

        /// <summary>
        /// Blueprint ���� UI
        /// </summary>
        private void DrawBlueprintManagementUI()
        {
            if (GUILayout.Button("Blueprints ����"))
            {
                RefreshBlueprints(factory);
                Debug.Log($"Blueprint ����: �� {factory.blueprints.Count}���� Blueprint�� ã�ҽ��ϴ�.");
            }

            EditUtility.SubjectLine(Color.gray, 2, "Actor ����");

            if (factory.blueprints != null && factory.blueprints.Count > 0)
            {
                string[] blueprintNames = factory.blueprints.Select(bp => bp.ActorName).ToArray();
                selectedIndex = EditorGUILayout.Popup("������ Actor ����", selectedIndex, blueprintNames);

                if (GUILayout.Button("������ Actors�� �߰�"))
                {
                    AddSelectedActorToList(factory, selectedIndex);
                }

                selectedBlueprintsList.DoLayoutList();

                if (GUILayout.Button("Scene�� ���� Actor ��ġ"))
                {
                    PlaceSelectedActors();
                }

                EditUtility.DrawLine(Color.gray, 1);

                if (GUILayout.Button("Scene�� ��� Actor ��ġ"))
                {
                    PlaceAllActors(factory);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Blueprints�� ��� �ֽ��ϴ�. ���� ��ư�� ���� ������Ʈ�ϼ���.", MessageType.Warning);
            }
        }

        /// <summary>
        /// Actor ���� UI
        /// </summary>
        private void DrawActorManagementUI()
        {
            EditUtility.SubjectLine(Color.gray, 2, "Actor ����");

            spawnedActorsList.DoLayoutList();

            // ���õ� Actor ���赵 ����
            if (GUILayout.Button("���� Actor ����"))
            {
                int selectedIndex = spawnedActorsList.index;
                if (selectedIndex >= 0 && selectedIndex < factory.MadeObjects.Count)
                {
                    RefreshBlueprints(factory);
                    factory.ApplyBlueprintToActor(selectedIndex);
                }
                else
                {
                    Debug.LogWarning("��ȿ���� ���� Actor�� ���õǾ����ϴ�.");
                }
            }

            EditUtility.DrawLine(Color.gray, 1);

            // ���õ� Actor ����
            if (GUILayout.Button("���� Actor ����"))
            {
                int selectedIndex = spawnedActorsList.index; // ���õ� �ε��� ��������

                if (selectedIndex >= 0 && selectedIndex < factory.MadeObjects.Count)
                {
                    GameObject selectedActor = factory.MadeObjects[selectedIndex];
                    factory.DestroyActor(selectedActor); // ���õ� ���� ����
                    UpdateSpawnedActorsList();
                    Repaint();
                }
                else
                {
                    Debug.LogWarning("���õ� Actor�� �����ϴ�!");
                }
            }

            EditUtility.DrawLine(Color.gray, 1);

            if (GUILayout.Button("��� Actor ����"))
            {
                factory.DestroyAllActors();
                UpdateSpawnedActorsList();
                Repaint();
            }
        }

        /// <summary>
        /// ���õ� Blueprint�� �����ϴ� ReorderableList �ʱ�ȭ
        /// </summary>
        private void InitializeBlueprintsList()
        {
            selectedBlueprintsList = new ReorderableList(selectedBlueprints, typeof(Blueprint_Actor), true, true, false, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "������ Actors"),
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
        /// ������ Actor�� �����ϴ� ReorderableList �ʱ�ȭ
        /// </summary>
        private void InitializeSpawnedActorsList()
        {
            spawnedActorsList = new ReorderableList(factory.MadeObjects.ToList(), typeof(GameObject), false, true, false, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "������ Actors"),
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
                // ����Ʈ �׸� ���� ǥ�� (���̾��Ű�� ����)
                onSelectCallback = list =>
                {
                    var actors = factory.MadeObjects;
                    if (list.index >= 0 && list.index < actors.Count && actors[list.index] != null)
                    {
                        EditorGUIUtility.PingObject(actors[list.index]); // ���̾��Ű ����
                    }
                }
            };
        }

        /// <summary>
        /// ���õ� Blueprint�� ������ ��Ͽ� �߰�
        /// </summary>
        private void AddSelectedActorToList(Factory_Actor factory, int index)
        {
            if (index < 0 || index >= factory.blueprints.Count)
            {
                Debug.LogWarning("��ȿ���� ���� Blueprint �����Դϴ�.");
                return;
            }

            selectedBlueprints.Add(factory.blueprints[index]);
        }

        /// <summary>
        /// ���õ� Actor ����� ���� ��ġ
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
        /// ��� Blueprint�� ������� Actor�� ���� ��ġ
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
        /// ������Ʈ ���� ��� Blueprint�� Factory_Actor�� ������Ʈ
        /// </summary>
        private void RefreshBlueprints(Factory_Actor factory)
        {
            string[] guids = AssetDatabase.FindAssets("t:Blueprint_Actor");
            var allBlueprints = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<Blueprint_Actor>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(blueprint => blueprint != null)
                .ToList();

            // �÷��̾ 0�� �ε����� ����
            factory.blueprints = allBlueprints.OrderBy(bp => bp.ActorName == "Player" ? 0 : 1).ToList();

            // ���� ���� ���� ����
            EditorUtility.SetDirty(factory);
        }

        /// <summary>
        /// ReorderableList�� Actor ���� - Delete Ű �Է�
        /// </summary>
        private void HandleDeleteKey()
        {
            Event e = Event.current; // ���� �̺�Ʈ ��������
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
            {
                // ReorderableList���� ���õ� �׸� �ε��� ��������
                int selectedIndex = spawnedActorsList.index;

                if (selectedIndex >= 0 && selectedIndex < factory.MadeObjects.Count)
                {
                    // ���õ� Actor ����
                    GameObject selectedActor = factory.MadeObjects[selectedIndex];
                    factory.DestroyActor(selectedActor);
                    UpdateSpawnedActorsList();
                    Repaint();
                }
                else
                {
                    Debug.LogWarning("���õ� Actor�� �����ϴ�.");
                }

                e.Use(); // �̺�Ʈ �Һ� (�ٸ� ������ ó������ �ʵ���)
            }
        }
        #endregion
    }
}