using UnityEngine;
using Noah;
using InnerSight_Kys;
using Unity.VisualScripting;

namespace Seti
{
    public enum AniState
    {
        Idle,
        Move,
        Dash,
        Attack,
        Stagger,
        Dead
    }

    public class Controller_Animator : MonoBehaviour
    {
        // 필드
        #region Variables
        // 컴포넌트
        protected Controller_Base controller;
        protected Damagable damagable;

        public AniState currentState;

        PlayerUseSkill useSkill;
        #endregion

        // 속성
        #region Properties
        public StateMachine<Controller_Animator> AniMachine { get; private set; }
        public Animator Animator { get; private set; }
        public Actor Actor { get; private set; }

        public float MoveSpeed { get; private set; }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            // 참조
            if (!TryGetComponent<Actor>(out var actor))
                actor = GetComponentInParent<Actor>();
            Actor = actor;

            if (!TryGetComponent<Controller_Base>(out var control))
                control = GetComponentInParent<Controller_Base>();
            controller = control;

            if (!TryGetComponent<Damagable>(out var damage))
                damage = GetComponentInParent<Damagable>();
            damagable = damage;

            // 이벤트 구독
            if (damagable != null)
                damagable.OnDeath += OnDie;

            // 애니메이션 컨트롤러 초기화
            Animator = GetComponent<Animator>();
            AniMachine = new StateMachine<Controller_Animator>(
                this,
                new AniState_Idle()
            );

            // 상태 추가
            AddStates();

            useSkill = transform.parent.GetComponent<PlayerUseSkill>();
        }

        private void Update()
        {
            MoveSpeed = CurrentSpeed();

            // FSM 업데이트
            AniMachine.Update(Time.deltaTime);
        }
        #endregion

        // 메서드
        #region Methods
        public void Initialize()
        {
            // 이건 모델 오브젝트를 루트 오브젝트의 자식으로 붙일 때만 사용한다
            /*transform.position = Actor.transform.position;
            transform.rotation = Actor.transform.rotation;*/
        }

        private void OnDie() => Actor.Condition.IsDead = true;

        private void AddStates()
        {
            // 누구나 죽는다
            AniMachine.AddState(new AniState_Die());

            if (controller.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                Move move = moveBehaviour as Move;
                if (move.HasStrategy<Move_Normal>() || move.HasStrategy<Move_Walk>() || move.HasStrategy<Move_Run>())
                    AniMachine.AddState(new AniState_Move());
            }

            if (controller.BehaviourMap.TryGetValue(typeof(Dash), out var dashBehaviour))
            {
                Dash dash = dashBehaviour as Dash;
                AniMachine.AddState(new AniState_Dash());
            }

            if (controller.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
            {
                Attack attack = attackBehaviour as Attack;
                if (attack.HasStrategy<Attack_Normal>() || attack.HasStrategy<Attack_Tackle>())
                    AniMachine.AddState(new AniState_Attack_Melee());

                if (attack.HasStrategy<Attack_Magic>())
                    AniMachine.AddState(new AniState_Attack_Magic());

                if (attack.HasStrategy<Attack_Slash_Double>())
                    AniMachine.AddState(new AniState_Slash_Double());

                if (attack.HasStrategy<Attack_Slash_Multiple>())
                    AniMachine.AddState(new AniState_Slash_Multiple());
            }

            if (controller.BehaviourMap.TryGetValue(typeof(Stagger), out var staggerBehaviour))
            {
                Stagger stagger = staggerBehaviour as Stagger;
                AniMachine.AddState(new AniState_Stagger());
            }
        }
        #endregion

        // 유틸리티
        //bool switchMove = false;
        public void MeleeAttackStart(int throwing = 0)
        {
            Actor.Condition.CurrentWeapon.BeginAttack(throwing != 0);
            Actor.Condition.IsAttack = true;
            Actor.Condition.AttackPoint = RayManager.Instance.RayToScreen();

            if (Actor.Controller.BehaviourMap.TryGetValue(typeof(Attack), out var attackBehaviour))
                if (attackBehaviour is Attack attack)
                    attack.OnAttackEnter();

            if (Actor is Player)
            {
                AudioManager.Instance.Play("Hitting Sound");
            }
        }
        public void MeleeAttackEnd()
        {
            Actor.Condition.CurrentWeapon.EndAttack();
            Actor.Condition.IsAttack = false;
        }
        public void MagicAttackEnd() => Actor.Condition.IsMagic = false;
        public void Slash_Double_End() => Actor.Condition.IsSlash_Double = false;
        public void Slash_Multiple_End() => Actor.Condition.IsSlash_Multiple = false;
        public void CantMoveDurAtk()
        {
            Actor.Condition.CanMove = false;
        }
        public void CanMoveAfterAtk()
        {
            Actor.Condition.CanMove = true;
        }

        [SerializeField]
        protected float forwardSpeed;
        protected float CurrentSpeed()
        {
            if (Actor && Actor.Condition.InAction)
            {
                if (Actor.Condition.IsMove && Actor.Condition.CanMove)
                    forwardSpeed = Mathf.Lerp(forwardSpeed, Actor.Rate_Movement, 20f * Time.deltaTime);
                else
                    forwardSpeed = forwardSpeed > 0.01f ? Mathf.Lerp(forwardSpeed, 0f, 10f * Time.deltaTime) : 0f;

                float moveEff = Actor.Condition.IsChase ? Actor.Magnification_WalkToRun : 1;
                return moveEff * forwardSpeed;
            }
            return 0f;
        }

        // 스킬 애니메이션 실행
        public void UseSkill()
        {
            useSkill.UseSkillAnimation();
            MagicAttackEnd();
        }

        public void PlayerSkillMove()
        {
            transform.parent.GetComponent<Condition_Player>().PlayerSetActive(true);

            Actor.Condition.IsMagic = false;
        }
    }
}