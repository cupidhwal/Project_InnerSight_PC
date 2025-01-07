using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    public abstract class Controller_Base : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        protected Actor actor;
        protected Dictionary<Type, IBehaviour> behaviourMap;    // �ൿ ���� (Ÿ�Կ� ���� �ൿ �ν��Ͻ�)
        #endregion

        // �������̽�
        #region Interface
        //public abstract IControl GetControlType();
        public void Initialize()
        {
            // Actor ����
            actor = GetComponent<Actor>();

            // Actor�� behaviours ����Ʈ���� �������� ����
            SetActorBehaviours(actor);
        }
        public void SetActorBehaviours(Actor actor)
        {
            behaviourMap = new Dictionary<Type, IBehaviour>();

            foreach (var behaviour in actor.Behaviours)
            {
                if (behaviour.behaviour == null) continue;

                // ��������� Initialize ȣ��
                behaviour.behaviour.Initialize(actor);

                var behaviourType = behaviour.behaviour.GetType();
                if (!behaviourMap.ContainsKey(behaviourType))
                {
                    behaviourMap.Add(behaviourType, behaviour.behaviour);
                }
            }
        }
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        protected virtual void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected virtual void FixedUpdate()
        {
            // Move �ൿ�� ������ FixedUpdate ȣ��
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                (moveBehaviour as Move)?.FixedUpdate();
            }
        }

        protected virtual void OnDestroy()
        {
            // �ൿ ���� ����
            behaviourMap?.Clear();
        }
        #endregion

        // �̺�Ʈ �޼���
        #region Event Methods
        protected virtual void OnCollisionEnter(Collision collision)
        {
            // Move �ൿ�� OnCollisionEnter ȣ��
            if (behaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                (moveBehaviour as Move)?.OnCollisionEnter(collision);
            }
        }
        #endregion
    }
}