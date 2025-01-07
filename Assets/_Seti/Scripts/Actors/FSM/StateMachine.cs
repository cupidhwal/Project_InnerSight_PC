using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// <T> State 정의
    /// </summary>
    [System.Serializable]
    public abstract class State<T>
    {
        // 필드
        #region
        protected StateMachine<T> stateMachine; // 현재 State가 등록 되어있는 Machine
        protected T context;                    // stateMachine을 가지고 있는 주체
        #endregion

        // 메서드
        #region Methods
        public void SetMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
            OnInitialized();
        }

        // 초기화 메서드 - 생성 후 1회 실행
        public virtual void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public virtual void OnEnter() { }

        // 상태 전환 시 State Exit에 1회 실행
        public virtual void OnExit() { }

        // 상태 전환 조건 메서드
        public virtual Type CheckTransitions() => null; // 기본적으로 전환 조건 없음

        // 상태 실행 중
        public abstract void Update();
        #endregion
    }

    public abstract class MonoState<T> : State<T> where T : MonoBehaviour
    {
        // MonoBehaviour와 연동된 State
        protected GameObject GameObject => (context as MonoBehaviour)?.gameObject;
        protected Transform Transform => (context as MonoBehaviour)?.transform;

        public override void OnInitialized()
        {
            Debug.Log($"State Initialized: {GetType().Name}");
        }
    }

    /// <summary>
    /// State를 관리하는 클래스
    /// </summary>
    public class StateMachine<T>
    {
        // 필드
        #region Variables
        public State<T> CurrentState { get; private set; }

        private T context;                                  // StateMachine을 가지고 있는 주체
        private Dictionary<Type, State<T>> states = new();  // 등록된 상태와 상태의 타입을 저장
        public event Action<State<T>> OnStateChanged;
        #endregion

        // 생성자
        #region Constructor
        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;
            AddState(initialState);
            CurrentState = initialState;
            CurrentState.OnEnter();
        }
        #endregion

        // 메서드
        #region Methods
        // State 등록
        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        // StateMachine에서 State의 업데이트 실행
        public void Update()
        {
            // 현재 상태의 전환 조건 검사
            var nextStateType = CurrentState?.CheckTransitions();

            // 전환 조건이 충족되면 상태 변경
            if (nextStateType != null && states.ContainsKey(nextStateType))
                ChangeState(nextStateType);

            CurrentState?.Update();
        }

        // CurrentState 체인지
        private void ChangeState(Type nextStateType)
        {
            CurrentState?.OnExit();
            CurrentState = states[nextStateType];
            CurrentState.OnEnter();
            OnStateChanged?.Invoke(CurrentState);
        }
        public R ChangeState<R>() where R : State<T>
        {
            // 현 상태와 새로운 상태 비교
            var newType = typeof(R);
            if (CurrentState.GetType() == newType) return CurrentState as R;

            // 상태 변경
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
외부 구독 예시
public class FSMObserver : MonoBehaviour
{
    private StateMachine<Actor> stateMachine;

    private void Start()
    {
        stateMachine = new StateMachine<Actor>(this, new IdleState());
        stateMachine.OnStateChanged += HandleStateChanged; // 이벤트 구독
    }

    private void HandleStateChanged(State<Actor> newState)
    {
        Debug.Log($"State changed to: {newState.GetType().Name}");
    }
}
 */

#region Dummy
/*
기본 클래스 - 제네릭
using System.Collections.Generic;

namespace Seti
{
    /// <summary>
    /// <T> State 정의
    /// </summary>
    [System.Serializable]
    public abstract class State<T>
    {
        // 필드
        #region
        protected StateMachine<T> stateMachine; // 현재 State가 등록 되어있는 Machine
        protected T context;                    // stateMachine을 가지고 있는 주체
        #endregion

        // 생성자
        #region Constructor
        public State() { }
        #endregion

        // 메서드
        #region Methods
        public void SetMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
            OnInitialized();
        }

        // 초기화 메서드 - 생성 후 1회 실행
        public virtual void OnInitialized() { }

        // 상태 전환 시 State Enter에 1회 실행
        public virtual void OnEnter() { }

        // 상태 전환 시 State Exit에 1회 실행
        public virtual void OnExit() { }

        // 상태 실행 중
        public abstract void Update(float deltaTime);
        #endregion
    }

    /// <summary>
    /// State를 관리하는 클래스
    /// </summary>
    public class StateMachine<T>
    {
        // 필드
        #region Variables
        private float elapsedTimeInState = 0.0f;// 현재 상태 지속 시간
        private T context;                      // StateMachine을 가지고 있는 주체

        // 등록된 상태와 상태의 타입을 저장
        private Dictionary<System.Type, State<T>> states = new();
        #endregion

        // 속성
        #region Properties
        public float ElapsedTimeInState => elapsedTimeInState;
        public State<T> CurrentState { get; private set; }
        public State<T> PreviousState { get; private set; }
        #endregion

        // 생성자
        #region Constructor
        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;
            AddState(initialState);
            CurrentState = initialState;
            CurrentState.OnEnter();
        }
        #endregion

        // 메서드
        #region Methods
        // State 등록
        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        // StateMachine에서 State의 업데이트 실행
        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;
            CurrentState.Update(deltaTime);
        }

        // CurrentState 체인지
        public R ChangeState<R>() where R : State<T>
        {
            // 현 상태와 새로운 상태 비교
            var newType = typeof(R);
            if (CurrentState.GetType() == newType)
            {
                return CurrentState as R;
            }

            // 상태 변경 이전
            CurrentState?.OnExit();
            PreviousState = CurrentState;

            // 상태 변경
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