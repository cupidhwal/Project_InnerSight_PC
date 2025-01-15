using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Seti
{
    // Actor가 가져야 할 Component
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Damagable))]

    /// <summary>
    /// Actor의 기본 정의
    /// </summary>
    public abstract class Actor : MonoBehaviour, IActor
    {
        // 필드
        #region Variables
        protected IControl control;
        [SerializeField]
        [HideInInspector]
        protected Blueprint_Actor blueprint;
        [SerializeField]
        [HideInInspector]
        protected Condition_Actor actorCondition;
        [SerializeField]
        [HideInInspector]
        protected List<Behaviour> behaviours = new();         // [직렬화 된 필드 - 읽기 전용 속성] 구조가 아니면 작동하지 않는다

        // 일반
        protected Controller_Animator animator;

        // 스탯
        [Header("Current Status")]
        [SerializeField]
        protected float health = 100f;
        [SerializeField]
        protected float attack = 10f;
        [SerializeField]
        protected float defend = 1f;
        #endregion

        // 속성
        #region Properties
        public Blueprint_Actor Origin => blueprint;
        public Condition_Actor ActorCondition => actorCondition;
        public List<Behaviour> Behaviours => behaviours;
        public Controller_Animator Controller_Animator => animator;

        // Default 스탯
        public float Health { get { return 100f; } }
        public float Attack { get { return 10f; } }
        public float Defend { get { return 1f; } }
        #endregion

        // 추상화
        #region Abstract
        protected abstract Condition_Actor CreateState();
        public abstract bool IsRelevant(Actor actor);
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            // 참조
            animator = GetComponentInChildren<Controller_Animator>();
        }
        #endregion

        // 스탯 적용
        #region Methods_Stats
        public void Update_Health(float heal) => health = heal;
        public void Update_Attack(float atk) => attack = atk;
        public void Update_Defend(float def) => defend = def;
        #endregion

        // 메서드
        #region Methods
        public void Initialize(Blueprint_Actor blueprint)
        {
            // Define Self
            this.blueprint = blueprint;

            // Check Actor State
            if (!actorCondition)
                actorCondition = CreateState();

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
            // 현재 Actor에 부착된 컨트롤러 탐색
            if (TryGetComponent<IController>(out var _))
            {
                control?.OnExit(this);
            }
            control = newControl;
            control.OnEnter(this);
        }

        public void SetStats()
        {

        }

        // 씬 내의 대적자 액터 가져오기
        public List<Actor> GetRelevantActors(IActor filter)
        {
            return FindObjectsByType<Actor>(FindObjectsSortMode.None)
                .Where(filter.IsRelevant)
                .ToList();
        }
        #endregion
    }
}