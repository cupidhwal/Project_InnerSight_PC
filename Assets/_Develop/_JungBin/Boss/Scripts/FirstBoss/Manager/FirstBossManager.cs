using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Seti;

namespace JungBin
{
    public class FirstBossManager : MonoBehaviour
    {
        #region Variables
        [Header("General Settings")]
        [SerializeField] private float turnSpeed = 30;              //보스의 회전 속도
        [SerializeField] private Transform detectedObj;            //돌진시 켜지는 레이의 시작점

        [Header("Attack Settings")]
        [SerializeField] private GameObject rushAttackBox;          //돌진시 켜지는 콜라이더 오브젝트
        [SerializeField] private BoxCollider throwAttackBox;        //던질때 켜지는 콜라이더
        [SerializeField] private GameObject attackBox;            //기본 공격시 켜지는 콜라이더
        [SerializeField] private ParticleSystem slashAttack;


        [Header("Detection Settings")]
        [SerializeField] private float detectionRange = 8f; //  최대 감지 거리
        [SerializeField] private float detectionAngle = 30f; // 레이의 시야각(좁은 각도)
        private bool isPlayerDetected = false;
        [SerializeField] private LayerMask playerLayer;

        private List<Transform> spawnPoints = new List<Transform>();
        private int lastAttack = -1;
        private bool isMaxHeight = false;
        public static bool isAttack { get; set; } = false; // 공격중인지 여부

        private Transform player;
        [SerializeField] private Animator animator;
        private NavMeshAgent navMeshAgent;
        #endregion


        private void Start()
        {
            //참조
            // 싱글톤 인스턴스를 통해 Player 가져오기
            if (BossStageManager.Instance == null)
            {
                Debug.LogError("BossStageManager instance not initialized!");
                return;
            }

            player = BossStageManager.Instance.Player?.transform;

            if (player == null)
            {
                Debug.LogError("Player GameObject is null in BossStageManager!");
            }
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (animator.GetBool("IsDeath") == true || player != null)
            {
                return;
            }

            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;
            if (!isAttack)
            {
                RotateTowardsPlayer(direction);
            }

            if (DetectPlayer() == false)
            {
                animator.SetBool("IsDetected", false);
            }
            else
            {
                animator.SetBool("IsDetected", true);
            }

            ManageDistanceToPlayer(distance);

            if (animator.GetBool("IsRun") == true)
            {
                //animator.applyRootMotion = false; // Root Motion 비활성화
                navMeshAgent.enabled = true;
                if (navMeshAgent.enabled == true)
                {
                    navMeshAgent.SetDestination(player.position);
                }
            }
            else if (animator.GetBool("IsRun") == false)
            {
                navMeshAgent.enabled = false;
                // animator.applyRootMotion = true; // Root Motion 활성화
            }
            ManageAttackBoxes();


            if (animator.GetBool("IsAttack02") == true)
            {
                if (DetectWall() == true)
                {
                    animator.SetBool("IsWall", true);
                }
            }
        }

        #region 일반적인 상태

        private void RotateTowardsPlayer(Vector3 direction) // 보스의 회전
        {
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        private void ManageDistanceToPlayer(float distance) // 거리가 멀어질경우 보스의 이동
        {
            animator.SetBool("IsFar", distance > detectionRange);
        }

        private bool DetectPlayer()    //시야에 플레이어가 없다면 이동
        {
            //Vector3 direction = player.position - detectedObj.position;
            Vector3 direction = new Vector3(player.position.x, detectedObj.position.y, player.position.z) - detectedObj.position;
            Vector3 directionToPlayer = direction.normalized;
            float distanceToPlayer = Vector3.Distance(detectedObj.position, player.position);

            // 거리와 각도를 확인
            if (distanceToPlayer <= detectionRange)
            {
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                if (angleToPlayer <= detectionAngle / 2)
                {
                    // 레이캐스트로 충돌 객체 확인
                    if (Physics.Raycast(detectedObj.position, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                    {

                        // 충돌한 객체가 플레이어인지 확인
                        if (hit.transform.CompareTag("Wall"))
                        {
                            return false; // 플레이어가 감지됨
                        }
                    }
                }
            }
            return true; // 플레이어가 감지되지 않음
        }

        #endregion

        #region 공격 상태
        public void SelectNextAttack()  //보스의 공격 패턴 결정(연속으로 같은 공격이 나오지는 않음)
        {
            int attackIndex;
            do
            {
                attackIndex = Random.Range(1, 4);
            } while (attackIndex == lastAttack);

            TriggerAttackAnimation(attackIndex);
            lastAttack = attackIndex;
        }

        private void TriggerAttackAnimation(int attackIndex)    // 결정된 공격 패턴을 애니메이션에게 전달
        {
            animator.SetTrigger($"Attack0{attackIndex}");
            animator.SetBool("Idle", false);
        }

        private void ManageAttackBoxes()    //공격시의 콜라이더 활성화
        {
            rushAttackBox.SetActive(animator.GetBool("IsAttack02"));
            throwAttackBox.enabled = animator.GetBool("IsAttack03");
        }

        public void OnAttackBox()
        {
            attackBox.SetActive(!attackBox.activeSelf);
            slashAttack.gameObject.SetActive(!slashAttack.gameObject.activeSelf);
            slashAttack.Play();
        }
        #endregion

        #region 돌진 공격
        private void RushAttack() //======================================================
        {

        }

        private bool DetectWall()    // 시야에 벽이 있으면 불값 리턴
        {
            Vector3 directionToWall = transform.forward; // 보스의 정면 방향
            float detectionRange = 1f; // 감지 거리

            // 레이캐스트로 충돌 객체 확인
            if (Physics.Raycast(transform.position, directionToWall, out RaycastHit hit, detectionRange))
            {
                if (hit.transform.CompareTag("Wall"))
                {
                    BrokenWall brokenWall = hit.transform.GetComponent<BrokenWall>();
                    brokenWall.RushToWall();
                    return true; // 벽이 감지됨
                }
            }

            return false; // 벽이 감지되지 않음
        }


        #endregion


    }
}
