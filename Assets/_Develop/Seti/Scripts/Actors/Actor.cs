using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Seti
{
    // Actor가 가져야 할 Component
    //[RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Damagable))]

    /// <summary>
    /// Actor의 기본 정의
    /// </summary>
    public abstract class Actor : MonoBehaviour
    {
        // 필드
        #region Variables
        protected IControl control;
        [SerializeField]
        [HideInInspector]
        protected Blueprint_Actor blueprint;
        [SerializeField]
        [HideInInspector]
        protected Condition_Actor condition;
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
        [SerializeField]
        protected float rate_Attack = 1f;
        [SerializeField]
        protected float rate_Movement = 4f;

        // 부가 기능
        [Header("CC Status")]
        [SerializeField]
        protected float stagger = 0.5f;   // 경직 시간

        [Header("Variables")]
        [SerializeField]
        protected float mag_WalkToRun = 1.5f;
        #endregion

        // 속성
        #region Properties
        public Blueprint_Actor Origin => blueprint;
        public Condition_Actor Condition => condition;
        public List<Behaviour> Behaviours => behaviours;
        public Controller_Animator Controller_Animator => animator;

        // 스탯 Default
        public float Health_Default => 100f;
        public float Attack_Default => 10f;
        public float Defend_Default => 1f;
        public float Rate_Attack_Default => 1f;
        public float Rate_Movement_Default => 4f;
        public float Stagger_Default => 0.5f;

        // 스탯 외부 참조
        public float Health => health;
        public float Attack => attack;
        public float Defend => defend;
        public float Rate_Attack => rate_Attack;
        public float Rate_Movement => rate_Movement;
        public float Stagger => stagger;

        // 걷기/달리기
        public float Magnification_WalkToRun => mag_WalkToRun;
        #endregion

        // 스탯 적용
        #region Methods_Stats
        // Load 한 스탯을 적용
        public void SetStats(float heal, float atk, float def, float r_atk, float r_mov, float stag = 0.5f)
        {
            Update_Health(heal);
            Update_Attack(atk);
            Update_Defend(def);
            Update_Rate_Attack(r_atk);
            Update_Rate_Movement(r_mov);
            Update_Stagger(stag);

            Damagable damagable = GetComponent<Damagable>();
            damagable.ResetDamage();
        }

        public void Update_Health(float heal) => health = heal;
        public void Update_Attack(float atk) => attack = atk;
        public void Update_Defend(float def) => defend = def;
        public void Update_Rate_Attack(float r_atk) => rate_Attack = r_atk;
        public void Update_Rate_Movement(float r_mov) => rate_Movement = r_mov;
        public void Update_Stagger(float stag) => stagger = stag;
        #endregion

        // 추상화
        #region Abstract
        protected abstract Condition_Actor CreateState();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            // 참조
            if (!TryGetComponent<Controller_Animator>(out var animator))
                animator = GetComponentInChildren<Controller_Animator>();
            this.animator = animator;
        }
        #endregion

        // 메서드
        #region Methods
        public void Initialize(Blueprint_Actor blueprint)
        {
            // Define Self
            this.blueprint = blueprint;

            // Check Actor State
            if (!condition)
                condition = CreateState();
            condition.Initialize();

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
        #endregion
    }
}