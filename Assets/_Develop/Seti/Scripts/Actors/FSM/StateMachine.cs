using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// <T> State ����
    /// </summary>
    [System.Serializable]
    public abstract class State<T>
    {
        // �ʵ�
        #region
        protected StateMachine<T> stateMachine; // ���� State�� ��� �Ǿ��ִ� Machine
        protected T context;                    // stateMachine�� ������ �ִ� ��ü
        #endregion

        // �޼���
        #region Methods
        public void SetMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
            OnInitialized();
        }

        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public virtual void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public virtual void OnEnter() { }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public virtual void OnExit() { }

        // ���� ��ȯ ���� �޼���
        public virtual Type CheckTransitions() => null; // �⺻������ ��ȯ ���� ����

        // ���� ���� ��
        public abstract void Update();
        #endregion
    }

    public abstract class MonoState<T> : State<T> where T : MonoBehaviour
    {
        // MonoBehaviour�� ������ State
        protected GameObject GameObject => (context as MonoBehaviour)?.gameObject;
        protected Transform Transform => (context as MonoBehaviour)?.transform;

        public override void OnInitialized()
        {
            Debug.Log($"State Initialized: {GetType().Name}");
        }
    }

    /// <summary>
    /// State�� �����ϴ� Ŭ����
    /// </summary>
    public class StateMachine<T>
    {
        // �ʵ�
        #region Variables
        public State<T> CurrentState { get; private set; }

        private T context;                                  // StateMachine�� ������ �ִ� ��ü
        private Dictionary<Type, State<T>> states = new();  // ��ϵ� ���¿� ������ Ÿ���� ����
        public event Action<State<T>> OnStateChanged;
        #endregion

        // ������
        #region Constructor
        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;
            AddState(initialState);
            CurrentState = initialState;
            CurrentState.OnEnter();
        }
        #endregion

        // �޼���
        #region Methods
        // State ���
        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        // StateMachine���� State�� ������Ʈ ����
        public void Update()
        {
            // ���� ������ ��ȯ ���� �˻�
            var nextStateType = CurrentState?.CheckTransitions();

            // ��ȯ ������ �����Ǹ� ���� ����
            if (nextStateType != null && states.ContainsKey(nextStateType))
                ChangeState(nextStateType);

            CurrentState?.Update();
        }

        // CurrentState ü����
        private void ChangeState(Type nextStateType)
        {
            CurrentState?.OnExit();
            CurrentState = states[nextStateType];
            CurrentState.OnEnter();
            OnStateChanged?.Invoke(CurrentState);
        }
        public R ChangeState<R>() where R : State<T>
        {
            // �� ���¿� ���ο� ���� ��
            var newType = typeof(R);
            if (CurrentState.GetType() == newType) return CurrentState as R;

            // ���� ����
            CurrentState?.OnExit();
            CurrentState = states[newType];
            CurrentState.OnEnter();
            OnStateChanged?.Invoke(CurrentState);

            return CurrentState as R;
        }
        #endregion
    }
}

/*
�ܺ� ���� ����
public class FSMObserver : MonoBehaviour
{
    private StateMachine<Actor> stateMachine;

    private void Start()
    {
        stateMachine = new StateMachine<Actor>(this, new IdleState());
        stateMachine.OnStateChanged += HandleStateChanged; // �̺�Ʈ ����
    }

    private void HandleStateChanged(State<Actor> newState)
    {
        Debug.Log($"State changed to: {newState.GetType().Name}");
    }
}
 */

#region Dummy
/*
�⺻ Ŭ���� - ���׸�
using System.Collections.Generic;

namespace Seti
{
    /// <summary>
    /// <T> State ����
    /// </summary>
    [System.Serializable]
    public abstract class State<T>
    {
        // �ʵ�
        #region
        protected StateMachine<T> stateMachine; // ���� State�� ��� �Ǿ��ִ� Machine
        protected T context;                    // stateMachine�� ������ �ִ� ��ü
        #endregion

        // ������
        #region Constructor
        public State() { }
        #endregion

        // �޼���
        #region Methods
        public void SetMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
            OnInitialized();
        }

        // �ʱ�ȭ �޼��� - ���� �� 1ȸ ����
        public virtual void OnInitialized() { }

        // ���� ��ȯ �� State Enter�� 1ȸ ����
        public virtual void OnEnter() { }

        // ���� ��ȯ �� State Exit�� 1ȸ ����
        public virtual void OnExit() { }

        // ���� ���� ��
        public abstract void Update(float deltaTime);
        #endregion
    }

    /// <summary>
    /// State�� �����ϴ� Ŭ����
    /// </summary>
    public class StateMachine<T>
    {
        // �ʵ�
        #region Variables
        private float elapsedTimeInState = 0.0f;// ���� ���� ���� �ð�
        private T context;                      // StateMachine�� ������ �ִ� ��ü

        // ��ϵ� ���¿� ������ Ÿ���� ����
        private Dictionary<System.Type, State<T>> states = new();
        #endregion

        // �Ӽ�
        #region Properties
        public float ElapsedTimeInState => elapsedTimeInState;
        public State<T> CurrentState { get; private set; }
        public State<T> PreviousState { get; private set; }
        #endregion

        // ������
        #region Constructor
        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;
            AddState(initialState);
            CurrentState = initialState;
            CurrentState.OnEnter();
        }
        #endregion

        // �޼���
        #region Methods
        // State ���
        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        // StateMachine���� State�� ������Ʈ ����
        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;
            CurrentState.Update(deltaTime);
        }

        // CurrentState ü����
        public R ChangeState<R>() where R : State<T>
        {
            // �� ���¿� ���ο� ���� ��
            var newType = typeof(R);
            if (CurrentState.GetType() == newType)
            {
                return CurrentState as R;
            }

            // ���� ���� ����
            CurrentState?.OnExit();
            PreviousState = CurrentState;

            // ���� ����
            CurrentState = states[newType];
            CurrentState.OnEnter();
            elapsedTimeInState = 0.0f;
            return CurrentState as R;
        }
        #endregion
    }
}
 */
#endregion