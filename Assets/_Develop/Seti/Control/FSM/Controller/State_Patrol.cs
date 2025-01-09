using System;
using UnityEngine;

namespace Seti
{
    public class State_Patrol : MonoState<Controller_FSM>
    {
        // �ʵ�
        #region Variables
        private float patrolTime;
        #endregion

        // �߻�
        #region Abstract
        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public override void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public override void OnEnter()
        {
            patrolTime = UnityEngine.Random.Range(5f, 15f);
        }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public override void OnExit() { }

        // ���� ��ȯ ���� �޼���
        public override Type CheckTransitions()
        {
            if (context.Detected)
                return typeof(State_Chase);

            else if (!context.Detected && context.StateMachine.ElapsedTime > patrolTime)
                return typeof(State_Idle);

            else return null;
        }

        // ���� ���� ��
        public override void Update(float deltaTime)
        {

        }
        #endregion

        // �޼���
        #region Methods
        #endregion
    }
}