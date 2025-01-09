using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Move : AniState_Base
    {
        // �������̵�
        #region Override
        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public override void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public override void OnEnter()
        {
            context.Animator.SetBool(isMove, true);
            base.OnEnter();
        }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public override void OnExit()
        {
            context.Animator.SetBool(isMove, false);
            base.OnExit();
        }

        // ���� ��ȯ ���� �޼���
        public override Type CheckTransitions()
        {
            if (!context.IsMove)
                return typeof(AniState_Idle);

            else if (context.IsDash)
                return typeof(AniState_Dash);
            
            else if (context.IsAttack)
                return typeof(AniState_Attack);
            
            return null;
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