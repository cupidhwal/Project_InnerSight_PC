using UnityEngine;

namespace Seti
{


    public class Controller_Animator : MonoBehaviour
    {
        // 필드
        #region Variables
        private StateMachine<Controller_Animator> aniMachine;

        // 속성
        public Animator Animator { get; private set; }
        public StateMachine<Controller_Animator> AniMachine => aniMachine;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected void Awake()
        {
            // 초기화
            Animator = GetComponent<Animator>();
            aniMachine = new StateMachine<Controller_Animator>(
                this,
                new AniState_Idle(this.GetComponentInParent<Actor>())
            );

            // 상태 추가
            aniMachine.AddState(new AniState_Move());
            aniMachine.AddState(new AniState_Dash());
            aniMachine.AddState(new AniState_Attack());
        }

        private void Update()
        {
            // FSM 업데이트
            aniMachine.Update();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (Animator)
            {
                // 현재 애니메이션 루트 포지션 가져오기
                //Vector3 rootPosition = Animator.bodyPosition;

                // 루트 포지션을 덮어씌워 이동을 고정
                Animator.bodyPosition = transform.position;

                // 루트 회전은 그대로 사용
                Animator.bodyRotation = Animator.bodyRotation;
            }
        }
        #endregion
    }
}

#region Dummy
/*using UnityEngine;

namespace Seti
{
    // 애니메이션 States 목록
    public enum AniStates
    {
        IDLE,
        IDLE_STAND,
        IDLE_ROTATE,
        JUMP,
        MOVE,
        MOVE_RUN_SWITCH,
        RIDE_BOARD_SWITCH,
        RIDE_BOARD_TURN
    }

    public static class AniString
    {
        public static string IsGround = "IsGround";
        public static string IsRotate = "IsRotate";
        public static string IsRight = "IsRight";
        public static string IsMove = "IsMove";
        public static string IsRun = "IsRun";
        public static string IsJump = "IsJump";
        public static string IsDash = "IsDash";
        public static string IsClimb = "IsClimb";
        public static string IsBoard = "IsBoard";
        public static string OnRight = "OnRight";
        public static string xVelocity = "xVelocity";
        public static string yVelocity = "yVelocity";
        public static string zVelocity = "zVelocity";
        public static string Velocity = "Velocity";
    }

    public class Controller_Animator : MonoBehaviour
    {
        // 필드
        #region Variables
        // 캐릭터 모델 동기화용 필드
        private Quaternion syncRotation = Quaternion.identity;
        private Quaternion targetRotion = Quaternion.identity;

        float x;
        float z;
        private Vector2 lerpInput;

        // 상태 변수
        private AniStates state;

        // 컴포넌트
        private Transform lookRoot;

        // 클래스
        private Actor actor;
        //private RidingBoard board;
        #endregion

        // 속성
        #region Properties
        public Animator Animator { get; private set; }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            Animator = GetComponent<Animator>();
            actor = GetComponentInParent<Actor>();
            lookRoot = actor.transform.Find("Head_Root");
        }

        private void Update()
        {
            AniSync();
            AniMove();
            AniRide();
        }
        #endregion

        // 메서드
        #region Methods
        public void ChangeState(AniStates newState)
        {
            state = newState;

            switch (state)
            {
                case AniStates.IDLE:
                    Animator.SetFloat(AniString.xVelocity, lerpInput.x);
                    Animator.SetFloat(AniString.zVelocity, lerpInput.y);
                    break;

                case AniStates.IDLE_STAND:
                    if (Animator.GetBool(AniString.IsRotate))
                        Animator.SetBool(AniString.IsRotate, false);
                    break;

                case AniStates.IDLE_ROTATE:
                    *//*if (player.playerLook.XLookInput > 3) Animator.SetBool(AniString.IsRight, true);
                    else if (player.playerLook.XLookInput < -3) Animator.SetBool(AniString.IsRight, false);
                    else return;*//*
                    if (!Animator.GetBool(AniString.IsRotate))
                        Animator.SetBool(AniString.IsRotate, true);
                    break;

                case AniStates.JUMP:
                    Animator.SetBool(AniString.IsJump, true);
                    break;

                case AniStates.MOVE:
                    Animator.SetFloat(AniString.xVelocity, lerpInput.x);
                    Animator.SetFloat(AniString.zVelocity, lerpInput.y);
                    break;

                case AniStates.MOVE_RUN_SWITCH:
                    Animator.SetBool(AniString.IsRun,
                        !Animator.GetBool(AniString.IsRun));
                    break;

                case AniStates.RIDE_BOARD_SWITCH:
                    //board = (RidingBoard)player.ridingGear;
                    Animator.SetBool(AniString.IsBoard,
                        !Animator.GetBool(AniString.IsBoard));
                    //Animator.SetBool(AniString.OnRight, board.IsRight);
                    break;

                case AniStates.RIDE_BOARD_TURN:
                    Animator.SetFloat(AniString.xVelocity, lerpInput.x);
                    Animator.SetFloat(AniString.zVelocity, lerpInput.y);
                    break;
            }
        }

        // 캐릭터 모델의 Root Transform 동기화 메서드
        public void AniSync()
        {
            if (Animator.GetBool(AniString.IsBoard))
            {
                targetRotion = actor.transform.rotation;
                syncRotation = Quaternion.Slerp(syncRotation, targetRotion, 0.2f);
                transform.SetPositionAndRotation(actor.transform.position - 0.15f * actor.transform.forward, syncRotation);
            }

            else
            {
                if (Animator.GetFloat(AniString.xVelocity) <= -0.5f) targetRotion = actor.transform.rotation * Quaternion.Euler(0, -67, 0);
                else if (Animator.GetFloat(AniString.xVelocity) >= 0.5f) targetRotion = actor.transform.rotation * Quaternion.Euler(0, 67, 0);
                else if (Mathf.Abs(Animator.GetFloat(AniString.xVelocity)) < 0.5f || Animator.GetBool(AniString.IsRotate)) targetRotion = actor.transform.rotation;

                syncRotation = Quaternion.Slerp(syncRotation, targetRotion, 0.2f);
                transform.SetPositionAndRotation(actor.transform.position, syncRotation);
            }
        }

        // 애니메이션용 지면 판정 메서드
        public void AniGround(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (Animator.GetBool(AniString.IsJump))
                    Animator.SetBool(AniString.IsJump, false);

                if (!Animator.GetBool(AniString.IsMove)) ChangeState(AniStates.IDLE);
                else ChangeState(AniStates.MOVE);
            }
        }

        // MOVE State의 블렌드 트리를 올바르게 사용하기 위한 메서드
        private void AniMove()
        {
            if (Animator.GetBool(AniString.IsMove))
            {
                ChangeState(AniStates.MOVE);
                *//*x = Mathf.Lerp(x, player.playerMove.MoveInput.x, 0.1f);
                z = Mathf.Lerp(z, player.playerMove.MoveInput.y, 0.1f);*//*
            }

            else
            {
                ChangeState(AniStates.IDLE);
                x = Mathf.Lerp(x, 0, 0.1f);
                z = Mathf.Lerp(z, 0, 0.1f);
            }
            
            lerpInput = new(x, z);
        }

        private void AniRide()
        {
            //if (player.ridingGear == null) return;

            if (Animator.GetBool(AniString.IsBoard))
            {
                *//*x = Mathf.Lerp(x, board.BoardDrive.MoveInput.x, 0.1f);
                z = Mathf.Lerp(z, board.BoardDrive.MoveInput.y, 0.1f);*//*
            }

            lerpInput = new(x, z);
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        private void OnAnimatorIK(int layerIndex)
        {
            if (Animator)
            {
                // 헤드의 IK 가중치 설정
                Animator.SetLookAtWeight(1.0f);  // 0~1 사이 값, 1일수록 완전하게 바라봄

                // 카메라가 따라갈 헤드 위치의 월드 좌표를 사용하여 바라보는 위치를 설정
                Vector3 lookAtPosition = lookRoot.position + lookRoot.forward * 10f;  // 카메라가 바라보는 방향을 기준으로 10 유닛 앞의 지점을 지정

                // 헤드가 바라볼 위치 설정
                Animator.SetLookAtPosition(lookAtPosition);
            }
        }
        #endregion
    }
}*/
#endregion