using System;

namespace Seti
{
    public class FSM_Move : MonoState<Controller_FSM>
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
        public override Type CheckTransitions() => null; // �⺻������ ��ȯ ���� ����

        // ���� ���� ��
        public override void Update()
        {

        }
        #endregion

        // �޼���
        #region Methods
        #endregion
    }
}