using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Actor�� �⺻ ����
    /// </summary>
    public abstract class Actor : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        protected IControl control;
        [SerializeField]
        [HideInInspector]
        protected Blueprint_Actor blueprint;
        [SerializeField]
        [HideInInspector]
        protected State_Actor actorState;
        [SerializeField]
        [HideInInspector]
        protected List<Behaviour> behaviours = new();         // [����ȭ �� �ʵ� - �б� ���� �Ӽ�] ������ �ƴϸ� �۵����� �ʴ´�

        // �Ӽ�
        public Blueprint_Actor Origin => blueprint;
        public State_Actor ActorState => actorState;
        public List<Behaviour> Behaviours => behaviours;
        #endregion

        // �߻�ȭ
        #region Abstract
        protected abstract State_Actor CreateState();
        #endregion

        // �޼���
        #region Methods
        public void Initialize(Blueprint_Actor blueprint)
        {
            // Define Self
            this.blueprint = blueprint;

            // Check Actor State
            if (!actorState)
                actorState = CreateState();

            // Check Animator Controller
            Animator animator = GetComponentInChildren<Animator>();
            var controller = animator.transform.GetComponent<Controller_Animator>()
                ?? animator.transform.gameObject.AddComponent<Controller_Animator>();

            // Define Control
            SwitchController();
        }

        public void AddBehaviour(IBehaviour be)
        {
            Behaviour behaviour = new(be);
            behaviour.behaviour.Initialize(this);
            Behaviours.Add(behaviour);
        }

        private void SwitchController()
        {
            switch (blueprint.controlType)
            {
                case ControlType.Input:
                    SwitchControlType(new Control_Input());
                    break;

                case ControlType.FSM:
                    SwitchControlType(new Control_FSM());
                    break;

                case ControlType.Stuff:
                    SwitchControlType(new Control_Stuff());
                    break;
            }
        }

        private void SwitchControlType(IControl newControl)
        {
            // ���� Actor�� ������ ��Ʈ�ѷ� Ž��
            if (TryGetComponent<IController>(out var _))
            {
                control?.OnExit(this);
            }
            control = newControl;
            control.OnEnter(this);
        }
        #endregion
    }
}