using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Actor Factory
    /// </summary>
    [CreateAssetMenu(fileName = "Factory_Actor", menuName = "Factory/Actor")]
    public class Factory_Actor : Factory<Factory_Actor>
    {
        // �ʵ�
        #region Variables
        [Tooltip("Actor�� ���赵�� �����ϴ� ����Ʈ")]
        public List<Blueprint_Actor> blueprints = new();
        #endregion

        // �ʱ�ȭ
        public override void Initialize()
        {
            base.Initialize();
            Debug.Log("Factory_Actor �߰� �ʱ�ȭ ���� ����");
        }

        // �޼���
        #region Methods
        /// <summary>
        /// ActorPrefab�� Blueprint�� ������� Actor GameObject�� �����ϰ� �ʱ�ȭ
        /// </summary>
        /// <param name="blueprint">Actor�� ������ ������ Blueprint</param>
        /// <param name="parent">�θ� Transform (�⺻��: null)</param>
        /// <returns>������ Actor GameObject</returns>
        public GameObject CreateActor(Blueprint_Actor blueprint, Transform parent = null)
        {
            if (blueprint == null || blueprint.actorPrefab == null)
            {
                Debug.LogError("Blueprint �Ǵ� ActorPrefab�� null�Դϴ�!");
                return null;
            }

            // Prefab �ν��Ͻ�ȭ
            GameObject actorObject;
            if (Application.isPlaying) actorObject = Instantiate(blueprint.actorPrefab, parent);
            else actorObject = PrefabUtility.InstantiatePrefab(blueprint.actorPrefab, parent) as GameObject;

            if (actorObject == null)
            {
                Debug.LogError("ActorPrefab�� �ν��Ͻ�ȭ�ϴ� �� �����߽��ϴ�!");
                return null;

            }

            // Actor ������Ʈ ��������
            if (!actorObject.TryGetComponent<Actor>(out var actor))
            {
                Debug.LogError($"ActorPrefab�� Actor ������Ʈ�� �����ϴ�! Prefab �̸�: {blueprint.ActorName}");
                DestroyImmediate(actorObject); // �ҿ����� ��ü ����
                return null;
            }

            UpdateBehaviours(blueprint, actor);
            actorObject.name = blueprint.ActorName; // �̸� ����

            // ����Ʈ�� �߰� �� ����
            AddToSpawnedActors(actorObject);
            return actorObject;
        }

        /// <summary>
        /// ���ŵ� ���赵�� �ൿ-���� ����
        /// </summary>
        public void UpdateBehaviours(Blueprint_Actor blueprint, Actor actor)
        {
            // ���� �ൿ �ʱ�ȭ
            actor.Behaviours.Clear();

            // �ൿ-���� ���� �� ����
            foreach (var beSt in blueprint.behaviourStrategies)
            {
                var newBehaviour = CreateNewBehaviour(beSt); // ������ ���ο� �ൿ ����
                actor.AddBehaviour(newBehaviour);
            }

            // ���� �ʱ�ȭ
            actor.Initialize(blueprint);
            EditorUtility.SetDirty(actor);
        }

        /// <summary>
        /// ���ŵ� ���赵�� �ൿ-���� ����
        /// </summary>
        private IBehaviour CreateNewBehaviour(BehaviourStrategyMapping mapping)
        {
            // ���ο� �ൿ ��ü ����
            var newBehaviour = Activator.CreateInstance(mapping.behaviour.GetType()) as IBehaviour;

            if (newBehaviour is IHasStrategy behaviourWithStrategy)
            {
                // Ȱ��ȭ�� ������ ���� ����
                var activeStrategies = mapping.strategies
                    .Where(s => s.isActive)
                    .Select(s => new Strategy
                    {
                        strategy = s.strategy,
                        isActive = s.isActive
                    }).ToList();

                behaviourWithStrategy.SetStrategies(activeStrategies);
            }

            return newBehaviour;
        }

        /// <summary>
        /// ������ ������ ���赵�� ����
        /// </summary>
        public void ApplyBlueprintToActor(int actorIndex)
        {
            if (actorIndex < 0 || actorIndex >= madeObjects.Count)
            {
                Debug.LogWarning("��ȿ���� ���� Actor �ε����Դϴ�.");
                return;
            }

            var actorObject = madeObjects[actorIndex];
            if (actorObject == null || !actorObject.TryGetComponent<Actor>(out var actor))
            {
                Debug.LogWarning("������ ������Ʈ�� Actor ������Ʈ�� �����ϴ�.");
                return;
            }

            if (Application.isPlaying)
            {
                Debug.LogWarning("��Ÿ�� �߿��� ���赵�� ������ �� �����ϴ�.");
                return;
            }

            // Blueprint�� ������ Actor�� ���������� ����
            UpdateBehaviours(actor.Origin, actor);
            Debug.Log($"'{actorObject.name}'�� ���赵�� ���ŵǾ����ϴ�.");
        }

        /// <summary>
        /// ������ ���͸� ����
        /// </summary>
        public void DestroyActor(GameObject actor)
        {
            if (madeObjects.Remove(actor))
            {
                DestroyImmediate(actor);
                SaveFactory();
            }
            else
            {
                Debug.LogWarning("�����Ϸ��� ���Ͱ� �������� �ʽ��ϴ�.");
            }
        }

        /// <summary>
        /// ��� ������ ���͸� ����
        /// </summary>
        public void DestroyAllActors()
        {
            foreach (var actor in madeObjects)
            {
                if (actor != null)
                {
                    DestroyImmediate(actor);
                }
            }
            madeObjects.Clear();
            SaveFactory();
        }

        /// <summary>
        /// ������ Actor�� ����Ʈ�� �߰��ϰ� ����
        /// </summary>
        private void AddToSpawnedActors(GameObject actor)
        {
            if (!madeObjects.Contains(actor))
            {
                madeObjects.Add(actor);
                SaveFactory();
            }
        }

        /// <summary>
        /// Factory ���� ����
        /// </summary>
        private void SaveFactory()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        #endregion
    }
}