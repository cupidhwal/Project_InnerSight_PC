using System;
using UnityEngine;

namespace Seti
{
    public class Player_Move : Player_Base_AniState
    {
        // �������̵�
        #region Override
        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public override void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public override void OnEnter()
        {
            context.Animator.SetBool(isMove, true);
        }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public override void OnExit()
        {
            context.Animator.SetBool(isMove, false);
        }

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