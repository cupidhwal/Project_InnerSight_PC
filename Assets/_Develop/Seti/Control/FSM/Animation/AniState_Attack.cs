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

        // �Ӽ�
        #region Properties
        private int AttackCombo => comboIndex++ % comboCount;
        #endregion

        // �������̵�
        #region Override
        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public override void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public override void OnEnter()
        {
            context.Animator.SetInteger(WhichAttack, AttackCombo);
            context.Animator.SetBool(isAttack, true);
            base.OnEnter();
        }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public override void OnExit()
        {
            context.Animator.SetBool(isAttack, false);
            base.OnExit();
        }

        // ���� ��ȯ ���� �޼���
        public override Type CheckTransitions()
        {
            if (!context.IsAttack && !context.IsMove)
                return typeof(AniState_Idle);

            else if (!context.IsAttack && context.IsMove)
                return typeof(AniState_Move);
            
            return null;
        }

        // ���� ���� ��
        public override void Update(float deltaTime)
        {

        }
        #endregion
    }
}