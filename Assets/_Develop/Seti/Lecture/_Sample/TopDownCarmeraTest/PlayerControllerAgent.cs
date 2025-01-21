using UnityEngine;
using UnityEngine.AI;

namespace MySampleEx
{
    /// <summary>
    /// 플레이어(Agent) 캐릭터 제어(이동, 회전, 애니메이션, ..) 관리 클레스
    /// </summary>
    public class PlayerControllerAgent : MonoBehaviour
    {
        #region Variables
        protected PlayerInputAgent m_Input;
        protected CharacterController m_CharCtrl;
        protected Animator m_Animator;
        protected NavMeshAgent m_Agent;
        protected Camera m_Camera;

        //이동 입력값 체크
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Agent.velocity.magnitude, 0f); }
        }

        protected bool m_IsGrounded = true;
        [SerializeField] protected LayerMask groundLayerMask;

        public GameObject clickEffecPrefab;

        //대기 상태로 보내기
        public float idleTimeout = 5f;              //이동상태 대기에서 5초가 지나면 대기 상태로 보낸다
        protected float m_idleTimer = 0f;           //타이머 카운드

        //애니메이션 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashGrounded = Animator.StringToHash("Grounded");        
        readonly int m_HashTimeoutIdle = Animator.StringToHash("TimeoutIdle");
        #endregion

        private void Awake()
        {
            //참조
            m_Input = GetComponent<PlayerInputAgent>();
            m_Animator = GetComponent<Animator>();
            m_CharCtrl = GetComponent<CharacterController>();

            m_Camera = Camera.main;

            m_Agent = GetComponent<NavMeshAgent>();
            m_Agent.updatePosition = false;             //m_Agent 의 위치값 적용하지 않는다
            m_Agent.updateRotation = true;              //m_Agent 의 회전값 적용한다
        }

        private void FixedUpdate()
        {
            CalculateForwardMovement();
            TimoutToIdle(); ;
        }

        void CalculateForwardMovement()
        {
            if(m_Input.Click)
            {
                //마우스 위치로 부터 맵상의 위치를 얻어온다
                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 100, groundLayerMask))
                {
                    m_Agent.SetDestination(hit.point);

                    if(clickEffecPrefab)
                    {
                        GameObject effectGo = Instantiate(clickEffecPrefab, hit.point + new Vector3(0f, 0.1f, 0f), clickEffecPrefab.transform.rotation);
                        Destroy(effectGo, 2f);
                    }
                }

                //초기화
                m_Input.Click = false;
            }
        }


        //이동상태의 대기에서 대기시간(5초)이 지나면 대기 상태로 보낸다
        void TimoutToIdle()
        {
            //입력값 체크(이동, 공격)
            bool inputDetected = IsMoveInput;

            //타이머 카운트
            if (m_IsGrounded && !inputDetected)
            {
                m_idleTimer += Time.deltaTime;
                if (m_idleTimer >= idleTimeout)
                {
                    m_Animator.SetTrigger(m_HashTimeoutIdle);

                    //초기화
                    m_idleTimer = 0;
                }
            }
            else
            {
                //초기화
                m_idleTimer = 0;
                m_Animator.ResetTrigger(m_HashTimeoutIdle);
            }

            //애니 입력값 설정
            m_Animator.SetBool(m_HashInputDetected, inputDetected);
        }

        private void OnAnimatorMove()
        {
            //캐릭터 위치 보정
            Vector3 position = m_Agent.nextPosition;
            m_Animator.rootPosition = position;
            transform.position = position;

            //이동
            if(m_Agent.remainingDistance > m_Agent.stoppingDistance)
            {
                m_CharCtrl.Move(m_Agent.velocity * Time.deltaTime);
            }
            else
            {
                m_CharCtrl.Move(Vector3.zero);
            }

            //애니메이터 적용
            m_Animator.SetFloat(m_HashForwardSpeed, m_Agent.velocity.magnitude);
            m_Animator.SetBool(m_HashGrounded, m_IsGrounded);
        }
    }
}
