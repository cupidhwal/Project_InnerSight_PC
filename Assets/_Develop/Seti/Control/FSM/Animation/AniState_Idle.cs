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
        public override void OnEnter()
        {
            context.transform.position = actorTransform.position;
            context.transform.rotation = actorTransform.rotation;
        }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public override void OnExit() { }

        // ���� ��ȯ ���� �޼���
        public override Type CheckTransitions() => null; // �⺻������ ��ȯ ���� ����

        // ���� ���� ��
        public override void Update()
        {

        }
        #endregion

        // ������
        #region Constructor
        public AniState_Idle(Actor actor) => actorTransform = actor.transform;
        #endregion
    }
}