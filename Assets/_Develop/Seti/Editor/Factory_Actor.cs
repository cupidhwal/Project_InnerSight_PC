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
        // 필드
        #region Variables
        [Tooltip("Actor의 설계도를 저장하는 리스트")]
        public List<Blueprint_Actor> blueprints = new();
        #endregion

        // 초기화
        public override void Initialize()
        {
            base.Initialize();
            Debug.Log("Factory_Actor 추가 초기화 로직 실행");
        }

        // 메서드
        #region Methods
        /// <summary>
        /// ActorPrefab과 Blueprint를 기반으로 Actor GameObject를 생성하고 초기화
        /// </summary>
        /// <param name="blueprint">Actor의 구성을 정의한 Blueprint</param>
        /// <param name="parent">부모 Transform (기본값: null)</param>
        /// <returns>생성된 Actor GameObject</returns>
        public GameObject CreateActor(Blueprint_Actor blueprint, Transform parent = null)
        {
            if (blueprint == null || blueprint.actorPrefab == null)
            {
                Debug.LogError("Blueprint 또는 ActorPrefab이 null입니다!");
                return null;
            }

            // Prefab 인스턴스화
            GameObject actorObject;
            if (Application.isPlaying) actorObject = Instantiate(blueprint.actorPrefab, parent);
            else actorObject = PrefabUtility.InstantiatePrefab(blueprint.actorPrefab, parent) as GameObject;

            if (actorObject == null)
            {
                Debug.LogError("ActorPrefab을 인스턴스화하는 데 실패했습니다!");
                return null;

            }

            // Actor 컴포넌트 가져오기
            if (!actorObject.TryGetComponent<Actor>(out var actor))
            {
                Debug.LogError($"ActorPrefab에 Actor 컴포넌트가 없습니다! Prefab 이름: {blueprint.ActorName}");
                DestroyImmediate(actorObject); // 불완전한 객체 삭제
                return null;
            }

            UpdateBehaviours(blueprint, actor);
            actorObject.name = blueprint.ActorName; // 이름 설정

            // 리스트에 추가 및 저장
            AddToSpawnedActors(actorObject);
            return actorObject;
        }

        /// <summary>
        /// 갱신된 설계도의 행동-전략 주입
        /// </summary>
        public void UpdateBehaviours(Blueprint_Actor blueprint, Actor actor)
        {
            // 기존 행동 초기화
            actor.Behaviours.Clear();

            // 행동-전략 생성 및 주입
            foreach (var beSt in blueprint.behaviourStrategies)
            {
                var newBehaviour = CreateNewBehaviour(beSt); // 완전히 새로운 행동 생성
                actor.AddBehaviour(newBehaviour);
            }

            // 액터 초기화
            actor.Initialize(blueprint);
            EditorUtility.SetDirty(actor);
        }

        /// <summary>
        /// 갱신된 설계도의 행동-전략 매핑
        /// </summary>
        private IBehaviour CreateNewBehaviour(BehaviourStrategyMapping mapping)
        {
            // 새로운 행동 객체 생성
            var newBehaviour = Activator.CreateInstance(mapping.behaviour.GetType()) as IBehaviour;

            if (newBehaviour is IHasStrategy behaviourWithStrategy)
            {
                // 활성화된 전략만 새로 생성
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
        /// 생성된 액터의 설계도를 갱신
        /// </summary>
        public void ApplyBlueprintToActor(int actorIndex)
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning("런타임 중에는 액터의 설계도를 갱신할 수 없습니다.");
                return;
            }

            if (actorIndex < 0 || actorIndex >= madeObjects.Count)
            {
                Debug.LogWarning("유효하지 않은 Actor 인덱스입니다.");
                return;
            }

            var actorObject = madeObjects[actorIndex];
            if (actorObject == null || !actorObject.TryGetComponent<Actor>(out var actor))
            {
                Debug.LogWarning("선택한 오브젝트에 Actor 컴포넌트가 없습니다.");
                return;
            }

            // Blueprint의 동작을 Actor와 독립적으로 적용
            UpdateBehaviours(actor.Origin, actor);
            Debug.Log($"'{actorObject.name}'의 설계도가 갱신되었습니다.");
        }

        /// <summary>
        /// 팩토리에 생성된 모든 Actor의 행동과 전략을 갱신
        /// </summary>
        public void ApplyBlueprintToAllActors()
        {
            for (int i = 0; i < madeObjects.Count; i++)
            {
                ApplyBlueprintToActor(i);
            }
        }

        /// <summary>
        /// 생성된 액터를 삭제
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
                Debug.LogWarning("삭제하려는 액터가 존재하지 않습니다.");
            }
        }

        /// <summary>
        /// 모든 생성된 액터를 삭제
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
        /// 생성된 Actor를 리스트에 추가하고 저장
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
        /// Factory 상태 저장
        /// </summary>
        private void SaveFactory()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        #endregion
    }
}