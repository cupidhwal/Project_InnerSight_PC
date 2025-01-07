using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// Move Function
    /// </summary>
    [System.Serializable]
    public class Move : IBehaviour, IHasStrategy
    {
        private enum MoveStrategies
        {
            Normal,
            //Run
        }

        // �ʵ�
        #region Variables
        private float speed_Walk = 4f;

        private Actor actor;
        [SerializeReference]
        private List<Strategy> strategies;
        private IMoveStrategy currentStrategy;
        private Vector2 moveInput; // �Է� ��

        // ���� ����
        private MoveStrategies moveStrategies;
        #endregion

        // �������̽�
        #region Interface
        // ���׷��̵�
        public void Upgrade(float increment)
        {
            float speed_Walk_Default = 4f;
            speed_Walk += increment * speed_Walk_Default / 100;

            /*float speed_Run_Default = 3.5f;
            speed_Run += increment * speed_Run_Default / 100;*/

            Initialize(actor);
        }

        // �ʱ�ȭ
        public void Initialize(Actor actor)
        {
            this.actor = actor;
            foreach (var mapping in strategies)
            {
                IMoveStrategy moveStrategy = mapping.strategy as IMoveStrategy;
                switch (moveStrategy)
                {
                    case Move_Normal:
                        moveStrategy.Initialize(actor, speed_Walk);
                        break;

                    /*case Move_Run:
                        moveStrategy.Initialize(actor, speed_Run);
                        break;*/
                }
            }

            // �ʱ� ���� ����
            var defaultStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy is Move_Normal);
            if (defaultStrategy != null)
            {
                ChangeStrategy(typeof(Move_Normal));
            }
            else if (strategies.Count > 0)
            {
                ChangeStrategy(strategies[0].strategy.GetType());
            }
            else
            {
                //Debug.LogWarning("Move ������ ���� �ʱ� ������ �������� ���߽��ϴ�.");
                ChangeStrategy(null);
            }
        }

        public Type GetBehaviourType() => typeof(Move);
        public Type GetStrategyType() => typeof(IMoveStrategy);

        // �ൿ ���� ����
        public void SetStrategies(IEnumerable<Strategy> strategies)
        {
            this.strategies = strategies.ToList(); // ���޹��� ���� ����Ʈ ����
        }

        // �ൿ ���� ����
        public void ChangeStrategy(Type strategyType)
        {
            var moveStrategy = CollectionUtility.FirstOrNull(strategies, s => s.strategy.GetType() == strategyType);
            if (moveStrategy != null)
            {
                currentStrategy = moveStrategy.strategy as IMoveStrategy;
            }
        }

        private void SwitchStrategy()
        {
            switch (moveStrategies)
            {
                case MoveStrategies.Normal:
                    ChangeStrategy(typeof(Move_Normal));
                    break;

                /*case MoveStrategies.Run:
                    ChangeStrategy(typeof(Move_Run));
                    break;*/
            }
        }
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        public void FixedUpdate()
        {
            currentStrategy?.Move(moveInput);
        }
        #endregion

        // �̺�Ʈ �ڵ鷯
        #region Event Handlers
        public void OnMovePerformed(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnMoveCanceled(InputAction.CallbackContext _)
        {
            moveInput = Vector2.zero;
        }

        public void OnRunStarted(InputAction.CallbackContext _)
        {
            // ü�� ���� ��� �������� ���� ���� �Ұ�
            //moveStrategies = MoveStrategies.Run;
            State_Common state = actor.ActorState as State_Common;
            if (!state.IsGrounded) return;

            //ChangeStrategy(typeof(Move_Run));
        }

        public void OnRunCanceled(InputAction.CallbackContext _)
        {
            // ü�� ���� ��� �������� ���� ���� �Ұ�
            moveStrategies = MoveStrategies.Normal;
            State_Common state = actor.ActorState as State_Common;
            if (!state.IsGrounded) return;

            ChangeStrategy(typeof(Move_Normal));
        }
        #endregion

        // �̺�Ʈ �޼���
        #region Event Methods
        public void OnCollisionEnter(Collision collision)
        {
            currentStrategy?.GetOverCurb(collision);
            SwitchStrategy();
        }
        #endregion
    }
}