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

            //transform.position = targetPosition; // 최종 위치 보정
            Debug.Log("목표 위치 도착, 공격 준비 시작");

            // 이동 후 플레이어가 왼쪽/오른쪽에 있는지 판단 후 애니메이션 설정
            DetermineAttackDirection();
        }

        // 이동 후 플레이어의 위치에 따라 공격 방향 결정 & 보스 회전
        private void DetermineAttackDirection()
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // 1️⃣ 보스가 플레이어를 바라보는 회전 값 계산 (기준 회전 값)
            Quaternion lookAtRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

            // 2️⃣ 현재 보스가 바라보는 방향과 비교하여 좌/우 판단
            Vector3 bossForward = transform.forward;
            float angle = Vector3.SignedAngle(bossForward, directionToPlayer, Vector3.up);

            // 3️⃣ 보스가 플레이어를 바라보는 상태에서 90도 회전한 값 계산
            if (angle < 0)
            {
                animator.SetFloat("AttackDirection", -1f); // 왼쪽 공격
                StartCoroutine(SmoothRotateBoss(lookAtRotation, 90f)); // 왼쪽으로 회전
                Debug.Log("왼쪽 공격 (기준 회전 값에서 +90도 회전)");
            }
            else
            {
                animator.SetFloat("AttackDirection", 1f); // 오른쪽 공격
                StartCoroutine(SmoothRotateBoss(lookAtRotation, -90f)); // 오른쪽으로 회전
                Debug.Log("오른쪽 공격 (기준 회전 값에서 -90도 회전)");
            }

            animator.SetTrigger("PrepareAttack"); // 공격 애니메이션 실행
        }

        // ✅ 보스가 플레이어를 바라보는 상태에서 90도 회전하는 코루틴
        private IEnumerator SmoothRotateBoss(Quaternion lookAtRotation, float angleOffset)
        {
            Quaternion targetRotation = lookAtRotation * Quaternion.Euler(0, angleOffset, 0); // 기준 회전 값에서 90도 회전 적용
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


        // 장애물이 있는지 확인하는 함수
        private bool IsObstacle(Vector3 target)
        {
            return Physics.Raycast(transform.position, (target - transform.position).normalized, stopDistance, obstacleLayer);
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