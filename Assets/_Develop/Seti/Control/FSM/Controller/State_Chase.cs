using System;
using UnityEngine;

namespace Seti
{
    public class State_Chase : MonoState<Controller_FSM>
    {
        // �߻�
        #region Abstract
        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public override void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public override void OnEnter() { }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public override void OnExit() { }

        // ���� ��ȯ ���� �޼���
        public override Type CheckTransitions()
        {
            if (!context.Detected)
                return typeof(State_Idle);

            else if (context.CanAttack)
                return typeof(State_Attack);

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