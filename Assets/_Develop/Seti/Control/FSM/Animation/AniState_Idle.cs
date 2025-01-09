using System;

namespace Seti
{
    public class AniState_Idle : AniState_Base
    {
        // �������̵�
        #region Override
        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public override void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public override void OnEnter() => base.OnEnter();

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public override void OnExit() => base.OnExit();

        // ���� ��ȯ ���� �޼���
        public override Type CheckTransitions()
        {
            if (context.IsMove)
                return typeof(AniState_Move);

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
    }
}