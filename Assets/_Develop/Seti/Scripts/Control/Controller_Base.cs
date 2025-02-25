using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    public abstract class Controller_Base : MonoBehaviour, IController
    {
        // 필드
        #region Variables
        protected Dictionary<Type, IBehaviour> behaviourMap;            // 행동 매핑 (타입에 따른 행동 인스턴스)
        #endregion

        // 속성
        #region Properties
        public Dictionary<Type, IBehaviour> BehaviourMap => behaviourMap;
        #endregion

        // 인터페이스
        #region Interface
        public abstract Type GetControlType();
        public virtual void Initialize()
        {
            // Actor의 behaviours 리스트에서 동적으로 매핑
            SetBehaviours(GetComponent<Actor>());
        }
        public virtual void SetBehaviours(Actor actor)
        {
            // 행동 매핑 초기화
            if (behaviourMap == null)
                behaviourMap = new();
            else behaviourMap.Clear();

            foreach (var mapping in actor.Blueprint.behaviourStrategies)
            {
                if (mapping.behaviour == null) continue;

                // 명시적으로 Initialize 호출
                mapping.behaviour.Initialize(actor);

                var behaviourType = mapping.behaviour.GetType();
                if (!behaviourMap.ContainsKey(behaviourType))
                {
                    behaviourMap.Add(behaviourType, mapping.behaviour);
                }
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
        }

        protected virtual void Update()
        {
            // Move 행동이 있으면 Update 호출
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                (moveBehaviour as Move)?.Update();
            }
        }

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void OnDestroy()
        {
            // 행동 매핑 해제
            behaviourMap?.Clear();
            behaviourMap = null;
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        protected virtual void OnCollisionEnter(Collision collision)
        {
            // Move 행동의 OnCollisionEnter 호출
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                (moveBehaviour as Move)?.OnCollisionEnter(collision);
            }
        }
        #endregion
    }
}