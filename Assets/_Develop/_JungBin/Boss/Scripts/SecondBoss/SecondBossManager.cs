using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace JungBin
{

    public class SecondBossManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float bossFlyTime = 0.7f;
        [SerializeField] private float turnSpeed = 30;              //보스의 회전 속도
        [SerializeField] private float detectionRange = 5f; //  최대 감지 거리

        private int lastAttack = -1;
        public static bool isAttack { get; set; } = false; // 공격중인지 여부

        //참조 변수
        [SerializeField] private Transform player;
        [SerializeField] private Animator animator;
        private NavMeshAgent navMeshAgent;

        [Header("이동 설정")]
        [SerializeField] private float stopDistance = 3f; // 보스가 이동할 거리
        [SerializeField] private LayerMask obstacleLayer; // 장애물 감지 레이어

        private Vector3 targetPosition; // 목표 위치 저장
        private bool lastMovedLeft = false; // 이전 이동 방향 저장 (true = 왼쪽, false = 오른쪽)

        private string Idle = "Idle";
        private string isFlyToPlayer = "IsFlyTP";
        private string isFlyNotToPlayer = "IsFlyNTP";
        private string isFar = "IsFar";
        private string isRun = "IsRun";
        #endregion

        // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        // Update is called once per frame
        void Update()
        {
            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;
            if (!isAttack)
            {
                RotateTowardsPlayer(direction);
            }

            ManageDistanceToPlayer(distance);

            if (animator.GetBool(isRun) == true)
            {
                //animator.applyRootMotion = false; // Root Motion 비활성화
                navMeshAgent.enabled = true;
                if (navMeshAgent.enabled == true)
                {
                    navMeshAgent.SetDestination(player.position);
                }
            }
            else if (animator.GetBool(isRun) == false)
            {
                navMeshAgent.enabled = false;
                // animator.applyRootMotion = true; // Root Motion 활성화
            }


            if (animator.GetBool(isFlyToPlayer) == true)
            {
                StartCoroutine(FlyToTarget(player.position, bossFlyTime));  // 1.5초 동안 이동
            }
            if(animator.GetBool(isFlyNotToPlayer) == true)
            {
                StartFlightPattern();
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
            animator.SetBool(isFar, distance > detectionRange);
        }

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
            animator.SetBool(Idle, false);
        }

        #endregion

        #region 플레이어에게 이동하는 비행 상태
        private IEnumerator FlyToTarget(Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                t = Mathf.Sin(t * Mathf.PI * 0.5f); // 출발 빠르고, 도착할수록 느려짐

                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            //transform.position = targetPosition; // 최종적으로 정확한 위치로 이동
            animator.SetBool(isFlyToPlayer, false);
        }
        #endregion

        #region 플레이어에게 이동하는게 아닌 비행 상태
        // 비행 패턴 시작 (이동할 방향을 결정함)
        public void StartFlightPattern()
        {
            Vector3 leftTarget = CalculateLeftMovement(); // 왼쪽 이동 목표 지점 계산
            Vector3 rightTarget = CalculateRightMovement(); // 오른쪽 이동 목표 지점 계산

            bool leftBlocked = IsObstacle(leftTarget); // 왼쪽에 장애물이 있는지 확인
            bool rightBlocked = IsObstacle(rightTarget); // 오른쪽에 장애물이 있는지 확인

            if (!leftBlocked && !rightBlocked)
            {
                // 양쪽 이동 가능할 경우, 이전 이동 방향과 반대로 이동
                if (lastMovedLeft)
                {
                    targetPosition = rightTarget;
                    lastMovedLeft = false;
                }
                else
                {
                    targetPosition = leftTarget;
                    lastMovedLeft = true;
                }
            }
            else if (!leftBlocked)
            {
                // 왼쪽 이동 가능하면 왼쪽으로 이동
                targetPosition = leftTarget;
                lastMovedLeft = true;
            }
            else if (!rightBlocked)
            {
                // 오른쪽 이동 가능하면 오른쪽으로 이동
                targetPosition = rightTarget;
                lastMovedLeft = false;
            }
            else
            {
                // 양쪽 다 이동 불가능할 경우, 다른 패턴 실행
                Debug.Log("양쪽 이동 불가, 대체 패턴 실행");
                TriggerAlternatePattern();
                return;
            }

            // 목표 위치로 이동 시작
            StartCoroutine(MoveToTarget(targetPosition, bossFlyTime));
            animator.SetBool(isFlyNotToPlayer, false);
        }

        // 왼쪽 이동 위치 계산
        private Vector3 CalculateLeftMovement()
        {
            Vector3 directionToPlayer = player.position - transform.position; // 플레이어 방향 계산
            Vector3 leftDirection = Quaternion.Euler(0, -45, 0) * directionToPlayer.normalized;
            return transform.position + leftDirection * directionToPlayer.magnitude; // 왼쪽 목표 위치 반환
        }

        // 오른쪽 이동 위치 계산
        private Vector3 CalculateRightMovement()
        {
            Vector3 directionToPlayer =  player.position - transform.position; // 플레이어 방향 계산
            Vector3 rightDirection = Quaternion.Euler(0, 45, 0) * directionToPlayer.normalized;
            return transform.position + rightDirection * directionToPlayer.magnitude; // 오른쪽 목표 위치 반환
        }

        // 장애물이 있는지 확인하는 함수
        private bool IsObstacle(Vector3 target)
        {
            return Physics.Raycast(transform.position, (target - transform.position).normalized, stopDistance, obstacleLayer);
        }

        // 목표 위치로 부드럽게 이동하는 코루틴 (Lerp 사용)
        private IEnumerator MoveToTarget(Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Sin((elapsedTime / duration) * Mathf.PI * 0.5f); // 처음 빠르고 끝에서 느려지는 이동
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            transform.position = targetPosition; // 최종 위치 보정
            Debug.Log("목표 위치 도착, 공격 준비 시작");
            animator.SetBool("PrepareAttack", true); // 공격 애니메이션 실행
        }

        // 양쪽이 막혀 있을 때 대체 패턴 실행
        private void TriggerAlternatePattern()
        {
            Debug.Log("양쪽이 막혀 있어 대체 패턴 실행");
            animator.SetBool("HiddenAttack", true); // 대체 패턴 애니메이션 실행
        }

        #endregion

    }
}