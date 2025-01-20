using UnityEngine;

namespace Seti
{
    public abstract class AniState_Base : MonoState<Controller_Animator>
    {
        // 필드
        #region Variables
        // Animator parameter
        protected int OnDash = Animator.StringToHash("OnDash");
        protected int isDeath = Animator.StringToHash("IsDeath");

        // float
        protected int Hash_VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        protected int Hash_AirborneVerticalSpeed = Animator.StringToHash("AirborneVerticalSpeed");
        protected int Hash_ForwardSpeed = Animator.StringToHash("ForwardSpeed");
        protected int Hash_AngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
        protected int Hash_HurtFromX = Animator.StringToHash("HurtFromX");
        protected int Hash_HurtFromY = Animator.StringToHash("HurtFromY");
        protected int Hash_StateTime = Animator.StringToHash("StateTime");
        protected int Hash_FootFall = Animator.StringToHash("FootFall");

        // int
        protected int Hash_RandomIdle = Animator.StringToHash("RandomIdle");

        // bool
        protected int Hash_Grounded = Animator.StringToHash("Grounded");
        protected int Hash_InputDetected = Animator.StringToHash("InputDetected");

        // trigger
        protected int Hash_TimeoutToIdle = Animator.StringToHash("TimeoutToIdle");
        protected int Hash_MeleeAttack = Animator.StringToHash("MeleeAttack");
        protected int Hash_Hurt = Animator.StringToHash("Hurt");
        protected int Hash_Death = Animator.StringToHash("Death");
        protected int Hash_Respawn = Animator.StringToHash("Respawn");
        #endregion

        // 오버라이드
        #region Override
        public override void OnEnter() => context.Initialize();
        public override void OnExit() => context.Initialize();
        public override void Update(float deltaTime)
        {
            context.Initialize();
            context.Animator.SetFloat(Hash_ForwardSpeed, context.MoveSpeed);

            /*if (IsOrientationUpdate() && context.IsMove)
            {
                UpdateOrientation();
            }*/
        }
        #endregion







        protected bool m_InAttack;                  //공격 여부 판단
        protected bool m_InCombo;                   //어택 상태 여부
        protected bool m_IsAnimatorTransitioning;               //상태 전환 중이냐?
        protected AnimatorStateInfo m_CurrentStateInfo;         //현재 애니 상태 정보
        protected AnimatorStateInfo m_NxetStateInfo;            //다음 애니 상태 정보
        protected AnimatorStateInfo m_PreviousCurrentStateInfo; //현재 애니 상태 정보 저장
        protected AnimatorStateInfo m_PreviousNxetStateInfo;    //다음 애니 상태 정보 저장
        protected bool m_PreviousIsAnimatorTransitioning;       //상태 전환 중이냐? 저장

        //애니메이션 상태 해시값
        readonly int m_HashLocomotion = Animator.StringToHash("Locomotion");
        readonly int m_HashAirborne = Animator.StringToHash("Airborne");
        readonly int m_HashLanding = Animator.StringToHash("Landing");
        readonly int m_HashEllenCombo1 = Animator.StringToHash("EllenCombo1");
        readonly int m_HashEllenCombo2 = Animator.StringToHash("EllenCombo2");
        readonly int m_HashEllenCombo3 = Animator.StringToHash("EllenCombo3");
        readonly int m_HashEllenCombo4 = Animator.StringToHash("EllenCombo4");

        public float maxForwardSpeed = 8f;          //플레이어 최고 이동 속도
        public float minTurnSpeed = 400f;           //플레이어 최저 회전 속도
        public float maxTurnSpeed = 1200f;          //플레이어 최고 회전 속도
        protected float m_AngleDiff;                //플레이어의 회전값과 타겟의 회전값의 차이 각도

        void UpdateOrientation()
        {
            //애니 입력값 설정
            context.Animator.SetFloat(Hash_AngleDeltaRad, m_AngleDiff * Mathf.Deg2Rad);
        }
        bool IsOrientationUpdate()
        {
            bool updateOrientationForLocomotion = (!m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLocomotion || m_NxetStateInfo.shortNameHash == m_HashLocomotion);
            bool updateOrientationForAirbon = (!m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashAirborne || m_NxetStateInfo.shortNameHash == m_HashAirborne);
            bool updateOrientationForLanding = (!m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLanding || m_NxetStateInfo.shortNameHash == m_HashLanding);



            Controller_Input controller = context.Actor.GetComponent<Controller_Input>();
            if (controller.BehaviourMap.TryGetValue(typeof(Move), out var moveBehaviour))
            {
                if (moveBehaviour is Move move)
                {
                    Vector3 localMovementDirection = new Vector3(move.MoveInput.x, 0f, move.MoveInput.y).normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(localMovementDirection);

                    Vector3 resultingForward = targetRotation * Vector3.forward;
                    float angleCurrent = Mathf.Atan2(context.transform.forward.x, context.transform.forward.z) * Mathf.Rad2Deg;
                    float angleTarget = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;
                    m_AngleDiff = Mathf.DeltaAngle(angleCurrent, angleTarget);
                }
            }

            return updateOrientationForLocomotion || /*updateOrientationForAirbon || updateOrientationForLanding ||*/ m_InCombo && !m_InAttack;
        }
    }
}