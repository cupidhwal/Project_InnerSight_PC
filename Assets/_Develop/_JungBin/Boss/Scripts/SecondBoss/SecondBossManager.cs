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
        [SerializeField] private float detectionRange = 8f; //  최대 감지 거리

        [SerializeField] private Transform slashSpawnPoint;
        [SerializeField] private GameObject slashAttack;

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
        private string isArrived = "IsArrived";
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

        #region 공격 상태
        public void ToggleAttack()
        {
            GameObject slashParticle = Instantiate(slashAttack, slashSpawnPoint.position, Quaternion.identity);

            Destroy(slashParticle, 2f );
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

                // 💡 도착 직전이면 애니메이션을 미리 전환
                if (Vector3.Distance(transform.position, targetPosition) < 1f) // 1f 이하일 때
                {
                    Debug.Log("거의 도착 → 패턴 즉시 실행");
                    animator.SetBool(isArrived, true); // 즉시 공격 패턴 실행
                    yield break;
                }

                yield return null;


            }

            //transform.position = targetPosition; // 최종적으로 정확한 위치로 이동
            animator.SetBool(isFlyToPlayer, false);
        }
        #endregion

        #region 플레이어에게 이동하는게 아닌 비행 상태
        // 비행 패턴 시작 (플레이어 기준 일정 거리 떨어진 랜덤 위치로 이동)
        public void StartFlightPattern()
        {
            targetPosition = GetRandomFlightPosition(); // 랜덤 비행 위치 계산

            // 장애물이 있는 경우 대체 패턴 실행
            if (IsObstacle(targetPosition))
            {
                Debug.Log("비행할 위치에 장애물이 있음, 대체 패턴 실행");
                TriggerAlternatePattern();
                return;
            }

            // 목표 위치로 이동 시작
            StartCoroutine(MoveToTarget(targetPosition, bossFlyTime));
            animator.SetBool(isFlyNotToPlayer, false);

        }

        // 플레이어 기준 5f 떨어진 랜덤 위치 반환
        private Vector3 GetRandomFlightPosition()
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 perpendicular = Vector3.Cross(Vector3.up, directionToPlayer).normalized; // 플레이어와 수직 방향

            // 왼쪽 또는 오른쪽 랜덤 선택
            float sign = Random.value > 0.5f ? 1f : -1f;
            Vector3 randomOffset = perpendicular * sign * 5f; // 좌우 5f 거리
            Vector3 targetPos = player.position + directionToPlayer * 5f + randomOffset; // 5f 앞 + 좌우 이동

            return targetPos;
        }

        // 목표 위치로 부드럽게 이동하는 코루틴
        private IEnumerator MoveToTarget(Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = transform.position;
            float distance = Vector3.Distance(startPosition, targetPosition);

            // 💡 처음부터 이동 거리가 너무 짧다면 즉시 패턴 전환
            if (distance < 2f) // 2f 기준 (조정 가능)
            {
                transform.position = targetPosition;
                Debug.Log("이동 거리 짧음 → 즉시 애니메이션 전환");

                animator.SetBool(isArrived, true); // 짧은 거리 이동 시 즉시 전환
                DetermineAttackDirection(); // 도착 후 공격 실행
                yield break; // 코루틴 종료
            }

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Sin((elapsedTime / duration) * Mathf.PI * 0.5f); // 처음 빠르고 끝에서 느려짐
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                // 💡 도착 직전이면 애니메이션을 미리 전환
                if (Vector3.Distance(transform.position, targetPosition) < 1f) // 1f 이하일 때
                {
                    Debug.Log("거의 도착 → 패턴 즉시 실행");
                    animator.SetBool(isArrived, true); // 즉시 공격 패턴 실행
                    DetermineAttackDirection(); // 도착 후 공격 실행
                    yield break;
                }

                yield return null;
            }

            //transform.position = targetPosition; // 최종 위치 보정
            Debug.Log("목표 위치 도착, 공격 준비 시작");

            animator.SetBool(isArrived, false); // 일반적인 이동 후 패턴 전환
            DetermineAttackDirection(); // 도착 후 공격 실행
        }



        // 이동 후 플레이어의 위치에 따라 공격 방향 결정 & 보스 회전
        private void DetermineAttackDirection()
        {
            // 🔹 플레이어 방향 계산
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // 🔹 현재 보스가 바라보는 방향
            Vector3 bossForward = transform.forward;

            // 🔹 예외 처리: bossForward 또는 directionToPlayer가 0이면 기본값으로 설정
            if (bossForward == Vector3.zero || directionToPlayer == Vector3.zero)
            {
                Debug.LogWarning("보스 방향 또는 플레이어 방향이 0! 기본 값 설정");
                directionToPlayer = Vector3.forward;
            }

            // 🔹 보스가 현재 바라보는 방향과 플레이어 방향 간의 각도 계산
            float angle = Vector3.SignedAngle(bossForward, directionToPlayer, Vector3.up);

            Debug.Log($"현재 보스 방향: {bossForward}, 플레이어 방향: {directionToPlayer}, angle: {angle}");

            // 🔹 0도일 경우 기본 방향 보정
            if (Mathf.Abs(angle) < 0.1f)
            {
                angle = Random.value > 0.5f ? -90f : 90f; // 랜덤으로 왼쪽 또는 오른쪽으로 회전
                Debug.Log("각도가 너무 작음 → 랜덤 방향 보정");
            }

            // 🔹 공격 방향 결정
            if (angle < 0)
            {
                animator.SetFloat("AttackDirection", -1f); // 왼쪽 공격
                StartCoroutine(SmoothRotateBoss(90f));
                Debug.Log("왼쪽 공격 실행");
            }
            else
            {
                animator.SetFloat("AttackDirection", 1f); // 오른쪽 공격
                StartCoroutine(SmoothRotateBoss(-90f));
                Debug.Log("오른쪽 공격 실행");
            }

            animator.SetTrigger("PrepareAttack"); // 공격 애니메이션 실행
        }


        // ✅ 보스의 현재 방향을 기준으로 보정된 90도 회전 적용
        private IEnumerator SmoothRotateBoss(float angleOffset)
        {
            // 현재 보스의 방향을 기준으로 보스가 플레이어를 바라보는 상태를 계산
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookAtRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

            // 🔹 "보스가 현재 바라보는 방향"을 기준으로 보정된 회전 적용
            Quaternion targetRotation = Quaternion.Euler(0, lookAtRotation.eulerAngles.y + angleOffset, 0);
            Quaternion startRotation = transform.rotation;

            float rotationDuration = 0.5f; // 회전 속도 조절 가능
            float elapsedTime = 0f;

            while (elapsedTime < rotationDuration)
            {
                elapsedTime += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
                yield return null;
            }

            transform.rotation = targetRotation; // 최종 보정
        }



        // ✅ 이동 경로에 장애물이 있는 경우 true 반환
        private bool IsObstacle(Vector3 target)
        {
            Vector3 directionToTarget = (target - transform.position).normalized; // 이동 방향
            float distanceToTarget = Vector3.Distance(transform.position, target); // 거리 계산

            // 이동 경로에 장애물이 있는지 체크
            if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, distanceToTarget, obstacleLayer))
            {
                Debug.Log($"이동 경로에 장애물 감지! 장애물: {hit.collider.name}");
                return true; // 장애물이 있음
            }

            return false; // 이동 가능
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