using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Attack : AniState_Base
    {
        // �ʵ�
        #region Variables
        private int comboIndex;
        private int comboCount = 2;
        #endregion

        // �������̵�
        #region Override
        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public override void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public override void OnEnter()
        {
            context.Animator.SetInteger(WhichAttack, AttackCombo());
            context.Animator.SetBool(isAttack, true);
        }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public override void OnExit()
        {
            context.Animator.SetBool(isAttack, false);
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
        private int AttackCombo() => comboIndex++ % comboCount;
        #endregion
    }
}