using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

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

        [Header("Jump Attack Settings")]
        [SerializeField] private GameObject rockPrefab;             //낙석시 스폰되는 오브젝트
        [SerializeField] private GameObject warningEffectPrefab;    //낙석시 스폰되는 경고 프리팹
        [SerializeField] private Transform spawnPointsParent;       //낙석 위치 부모 오브젝트
        [SerializeField] private float spawnHeight = 10f;           //낙석시 스폰되는 오브젝트의 스폰 위치
        [SerializeField] private int rocksPerBatch = 2;             //낙석이 한번에 떨어지는 오브젝트 갯수
        [SerializeField] private float spawnInterval = 0.4f;        //낙석의 시간 간격
        [SerializeField] private int totalBatches = 2;              //낙석이 몇번 떨어지는지 설정
        [SerializeField] private float warningDuration = 1f;      //경고 프리팹 지속시간

        [Header("Jump Settings")]
        [SerializeField] private Transform raycastOffsetObj;        //바닥 감지할 오브젝트 위치
        [SerializeField] private float raycastOffset = 0.15f;       //바닥 감지 거리
        [SerializeField] private float jumpSpeed = 10f;             //점프와 하강에 필요한 속도
        [SerializeField] private float maxHeight = 10f;             //점프 제한 높이
        [SerializeField] private LayerMask groundLayer;             
        [SerializeField] private GameObject jumpAttackBox;          //하강시 켜지는 콜라이더 오브젝트

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
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        #endregion

        private void Start()
        {
            //참조
            player = GameManager.Instance.Player.transform;
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            foreach (Transform child in spawnPointsParent)
            {
                spawnPoints.Add(child);
            }

            jumpAttackBox.SetActive(false);
        }

        private void Update()
        {
            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;
            if (!isAttack)
            {
                RotateTowardsPlayer(direction);
            }

            if(DetectPlayer() == false)
            {
                animator.SetBool("IsDetected", false);
            }
            else
            {
                animator.SetBool("IsDetected", true);
            }

            ManageDistanceToPlayer(distance);

            if(animator.GetBool("IsRun") == true)
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

            if (animator.GetBool("IsJump"))
            {
                HandleJumpAttack();
            }

            if(animator.GetBool("IsAttack02") == true) //======================================================
            {
                if(DetectWall() == true)
                {
                    animator.speed = 0.2f;
                    Debug.Log("================");
                }
                else
                {
                    animator.speed = 1f;
                }
            }
            else
            {
                animator.speed = 1f;
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
                    // 디버그용 레이 시각화
                    //Debug.DrawLine(detectedObj.position, detectedObj.position + directionToPlayer * distanceToPlayer, Color.green);
                    Debug.Log($"레이 오브젝트 : {detectedObj}");
                    // 레이캐스트로 충돌 객체 확인
                    if (Physics.Raycast(detectedObj.position, directionToPlayer, out RaycastHit hit, distanceToPlayer + 2, playerLayer))    
                    {
                        
                        Debug.Log($"Ray hit: {hit.transform.name}");
                        // 충돌한 객체가 플레이어인지 확인
                        if (hit.transform.CompareTag("Player"))
                        {                            
                            return true; // 플레이어가 감지됨
                        }
                    }
                }
            }
            // 감지 실패 시 디버그 레이            
            //Debug.DrawLine(detectedObj.position, detectedObj.position + directionToPlayer, Color.red);
            return false; // 플레이어가 감지되지 않음
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 direction = new Vector3(player.position.x, detectedObj.position.y, player.position.z) - detectedObj.position;
            Vector3 directionToPlayer = direction.normalized;
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(detectedObj.position, directionToPlayer);
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
        }
        #endregion

        #region 돌진 공격
        private void RushAttack() //======================================================
        {

        }

        private bool DetectWall()    // 시야에 벽이 있으면 불값 리턴
        {
            Vector3 directionToWall = transform.forward; // 보스의 정면 방향
            float detectionRange = 5f; // 감지 거리

            // 레이캐스트로 충돌 객체 확인
            if (Physics.Raycast(transform.position, directionToWall, out RaycastHit hit, detectionRange))
            {
                if (hit.transform.CompareTag("Wall"))
                {
                    return true; // 벽이 감지됨
                }
            }

            return false; // 벽이 감지되지 않음
        }


        #endregion

        #region 점프 공격
        public void StartJumpAttack()   //애니메이션 이벤트 함수
        {
            animator.SetBool("IsJump", true);
        }

        private void HandleJumpAttack() //점프시 상승 및 하강 담당
        {
            if (!isMaxHeight)
            {
                // 상승
                transform.position += Vector3.up * jumpSpeed * Time.deltaTime;

                if (transform.position.y >= maxHeight)
                {
                    isMaxHeight = true;
                }
            }
            else
            {
                // 유지
                if (animator.GetBool("IsAttack01"))
                {
                    JumpTowardsPlayer();
                }
                // 하강
                else
                {
                    transform.position += Vector3.down * jumpSpeed * 3f * Time.deltaTime;
                    jumpAttackBox.SetActive(true);

                    if (IsGrounded())
                    {
                        Debug.Log($"현재 높이 : {transform.position.y}");
                        animator.SetBool("IsJump", false);
                        isMaxHeight = false;
                        jumpAttackBox.SetActive(false);
                    }
                }
            }
        }

        private void JumpTowardsPlayer() //유지중일때 플레이어의 위치로 이동
        {
            // 플레이어의 XZ 방향으로 이동 (Y축 제외)
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Y축 이동 제거
            transform.position += direction * jumpSpeed * 5f * Time.deltaTime;

            // 플레이어 지점에 도달하면 유지 종료
            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                 new Vector3(player.position.x, 0, player.position.z)) < 1f)
            {
                animator.SetBool("IsAttack01", false); // 유지 종료, 하강으로 전환
            }
        }

        private bool IsGrounded()   // 하강시 바닥 감지
        {
            return Physics.Raycast(raycastOffsetObj.position, Vector3.down, raycastOffset, groundLayer);
        }

        #region 낙석 패턴
        public void OnLanding()  // 착지시 애니메이션 이벤트 함수
        {
            StartCoroutine(SpawnRocksWithInterval());
        }

        private IEnumerator SpawnRocksWithInterval()
        {
            for (int batch = 0; batch < totalBatches; batch++)
            {
                yield return StartCoroutine(ShowWarningsAndSpawnRocks());
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private IEnumerator ShowWarningsAndSpawnRocks() // 낙석시 경고표시 소환
        {
            int spawnedCount = 0;
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnedCount >= rocksPerBatch) break;

                GameObject warning = Instantiate(warningEffectPrefab, spawnPoint.position, Quaternion.identity);
                Destroy(warning, warningDuration);
                spawnedCount++;
            }

            yield return new WaitForSeconds(warningDuration);

            SpawnRocks();
        }

        private void SpawnRocks()   // 낙석
        {
            int spawnedCount = 0;
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnedCount >= rocksPerBatch) break;

                Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnHeight, spawnPoint.position.z);
                Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
                spawnedCount++;
            }
        }
        #endregion
        #endregion
    }
}
