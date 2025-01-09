using System;
using UnityEngine;

namespace Seti
{
    public class AniState_Dash : AniState_Base
    {
        // �������̵�
        #region Override
        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public override void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public override void OnEnter()
        {
            context.Animator.SetBool(isMove, true);
            context.Animator.SetBool(isDash, true);
            base.OnEnter();
        }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public override void OnExit()
        {
            context.Animator.SetBool(isMove, false);
            context.Animator.SetBool(isDash, false);
            base.OnExit();
        }

        // ���� ��ȯ ���� �޼���
        public override Type CheckTransitions()
        {
            if (!context.IsDash && context.IsMove)
                return typeof(AniState_Move);
            
            else if (!context.IsDash && !context.IsMove)
                return typeof(AniState_Idle);
            
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