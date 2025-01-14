using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    public abstract class Controller_Base : MonoBehaviour
    {
        // 필드
        #region Variables
        public Actor Actor { get; protected set; }
        protected Dictionary<Type, IBehaviour> behaviourMap;    // 행동 매핑 (타입에 따른 행동 인스턴스)
        #endregion

        // 인터페이스
        #region Interface
        public void Initialize()
        {
            // Actor 참조
            Actor = GetComponent<Actor>();

            // Actor의 behaviours 리스트에서 동적으로 매핑
            SetActorBehaviours(Actor);
        }
        public void SetActorBehaviours(Actor actor)
        {
            behaviourMap = new();

            foreach (var behaviour in actor.Behaviours)
            {
                if (behaviour.behaviour == null) continue;

                // 명시적으로 Initialize 호출
                behaviour.behaviour.Initialize(actor);

                var behaviourType = behaviour.behaviour.GetType();
                if (!behaviourMap.ContainsKey(behaviourType))
                {
                    behaviourMap.Add(behaviourType, behaviour.behaviour);
                }
            }
        }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Awake()
        {
            Initialize();
        }

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