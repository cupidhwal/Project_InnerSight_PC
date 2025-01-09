using UnityEngine;

namespace Seti
{


    public class Controller_Animator : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        private StateMachine<Controller_Animator> aniMachine;

        // �Ӽ�
        public Animator Animator { get; private set; }
        public StateMachine<Controller_Animator> AniMachine => aniMachine;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        protected void Awake()
        {
            // �ʱ�ȭ
            Animator = GetComponent<Animator>();
            aniMachine = new StateMachine<Controller_Animator>(
                this,
                new AniState_Idle(this.GetComponentInParent<Actor>())
            );

            // ���� �߰�
            aniMachine.AddState(new AniState_Move());
            aniMachine.AddState(new AniState_Dash());
            aniMachine.AddState(new AniState_Attack());
        }

        private void Update()
        {
            // FSM ������Ʈ
            aniMachine.Update();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (Animator)
            {
                // ���� �ִϸ��̼� ��Ʈ ������ ��������
                //Vector3 rootPosition = Animator.bodyPosition;

                // ��Ʈ �������� ����� �̵��� ����
                Animator.bodyPosition = transform.position;

                // ��Ʈ ȸ���� �״�� ���
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
    // �ִϸ��̼� States ���
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
        // �ʵ�
        #region Variables
        // ĳ���� �� ����ȭ�� �ʵ�
        private Quaternion syncRotation = Quaternion.identity;
        private Quaternion targetRotion = Quaternion.identity;

        float x;
        float z;
        private Vector2 lerpInput;

        // ���� ����
        private AniStates state;

        // ������Ʈ
        private Transform lookRoot;

        // Ŭ����
        private Actor actor;
        //private RidingBoard board;
        #endregion

        // �Ӽ�
        #region Properties
        public Animator Animator { get; private set; }
        #endregion

        // ������ ����Ŭ
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

        // �޼���
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

        // ĳ���� ���� Root Transform ����ȭ �޼���
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

        // �ִϸ��̼ǿ� ���� ���� �޼���
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

        // MOVE State�� ���� Ʈ���� �ùٸ��� ����ϱ� ���� �޼���
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

        // �̺�Ʈ �޼���
        #region Event Methods
        private void OnAnimatorIK(int layerIndex)
        {
            if (Animator)
            {
                // ����� IK ����ġ ����
                Animator.SetLookAtWeight(1.0f);  // 0~1 ���� ��, 1�ϼ��� �����ϰ� �ٶ�

                // ī�޶� ���� ��� ��ġ�� ���� ��ǥ�� ����Ͽ� �ٶ󺸴� ��ġ�� ����
                Vector3 lookAtPosition = lookRoot.position + lookRoot.forward * 10f;  // ī�޶� �ٶ󺸴� ������ �������� 10 ���� ���� ������ ����

                // ��尡 �ٶ� ��ġ ����
                Animator.SetLookAtPosition(lookAtPosition);
            }
        }
        #endregion
    }
}*/
#endregion