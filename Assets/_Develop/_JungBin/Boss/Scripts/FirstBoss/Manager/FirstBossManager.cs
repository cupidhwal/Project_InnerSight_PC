using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Linq;
using Seti;

namespace JungBin
{
    public class FirstBossManager : MonoBehaviour
    {
        #region Variables
        [Header("General Settings")]
        [SerializeField] private float turnSpeed = 30; // 보스의 회전 속도
        [SerializeField] private Transform detectedObj; // 돌진 시 켜지는 레이의 시작점

        [Header("Attack Settings")]
        [SerializeField] private GameObject rushAttackBox;
        private BoxCollider rushCollider;
        [SerializeField] private BoxCollider throwAttackBox;
        [SerializeField] private GameObject smashAttackBox;
        [SerializeField] private ParticleSystem slashAttack;
        [SerializeField] private GameObject warningEffectPrefab;
        [SerializeField] private float warningDuration;
        [SerializeField] private float secondWarningDuration;
        [SerializeField] private float Duration;
        [SerializeField] private float warningLength = 5f;
        private bool canSpawn = true;

        [Header("Detection Settings")]
        [SerializeField] private float detectionRange = 8f;
        [SerializeField] private float detectionAngle = 30f;

        private int lastAttack = -1;
        public static bool isAttack { get; set; } = false;

        [SerializeField] private Transform player;
        [SerializeField] private Animator animator;
        private NavMeshAgent navMeshAgent;
        #endregion

        private void Start()
        {
            if (BossStageManager.Instance == null)
            {
                Debug.LogError("BossStageManager instance not initialized!");
                return;
            }

            player = BossStageManager.Instance?.Player?.transform;
            if (player == null)
            {
                Debug.LogError("Player GameObject is null in BossStageManager!");
            }

            rushCollider = rushAttackBox.GetComponent<BoxCollider>();

            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (animator.GetBool("IsDeath")) return;
            if (player == null)
            {
                animator.SetBool("PlayerDie", true);
                return;
            }

            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            if (!isAttack) RotateTowardsPlayer(direction);

            animator.SetBool("IsDetected", !IsBlockedByWall());
            ManageDistanceToPlayer(distance);

            if (animator.GetBool("IsRun"))
            {
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                navMeshAgent.enabled = false;
            }

            ManageAttackBoxes();

            if (animator.GetBool("IsAttack02"))
            {
                if (DetectObstacle("Wall", out _)) animator.SetBool("IsWall", true);
                if (DetectObstacle("BWall", out RaycastHit hit))
                {
                    animator.SetBool("IsWall", true);
                    animator.SetBool("IsBWall", true);
                    hit.transform.GetComponent<BrokenWall>()?.RushToWall();
                }
            }

            if(animator.GetBool("IsAttack01") || animator.GetBool("IsAttack03"))
            {
                animator.applyRootMotion = false;
            }
            else if (!animator.GetBool("IsAttack01") && !animator.GetBool("IsAttack03"))
            {
                animator.applyRootMotion = true;
            }
        }

        #region 일반적인 상태
        private void RotateTowardsPlayer(Vector3 direction)
        {
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        private void ManageDistanceToPlayer(float distance)
        {
            animator.SetBool("IsFar", distance > detectionRange);
        }

        private bool IsBlockedByWall()
        {
            Vector3 direction = new Vector3(player.position.x, detectedObj.position.y, player.position.z) - detectedObj.position;
            Vector3 directionToPlayer = direction.normalized;
            float distanceToPlayer = Vector3.Distance(detectedObj.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                if (angleToPlayer <= detectionAngle / 2)
                {
                    if (Physics.Raycast(detectedObj.position, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                    {
                        return hit.transform.CompareTag("Wall");
                    }
                }
            }
            return false;
        }
        #endregion

        #region 공격 상태
        public void SelectNextAttack()
        {
            int[] possibleAttacks = { 1, 2, 3 };
            possibleAttacks = possibleAttacks.Where(a => a != lastAttack).ToArray();
            int attackIndex = possibleAttacks[Random.Range(0, possibleAttacks.Length)];
            TriggerAttackAnimation(attackIndex);
            lastAttack = attackIndex;
        }

        private void TriggerAttackAnimation(int attackIndex)
        {
            animator.SetTrigger($"Attack0{attackIndex}");
            animator.SetBool("Idle", false);
        }

        private void ManageAttackBoxes()
        {
            //rushAttackBox.SetActive(animator.GetBool("IsAttack02"));
            rushAttackBox.transform.GetComponent<BoxCollider>().enabled = animator.GetBool("IsAttack02");
            rushAttackBox.transform.GetChild(0).GetComponent<BoxCollider>().enabled = animator.GetBool("IsAttack02");
/*            rushCollider.enabled = animator.GetBool("IsAttack02");
            rushCollider.transform.GetChild(0).GetComponent<BoxCollider>().enabled = animator.GetBool("IsAttack02");*/
        }

        public void ToggleAttack(bool isActive)
        {
            BoxCollider[] colliders = smashAttackBox.GetComponents<BoxCollider>();
            foreach (BoxCollider col in colliders)
            {
                col.enabled = isActive;
            }
            slashAttack.gameObject.SetActive(isActive);
            if (isActive) slashAttack.Play();
        }

        public void OnAttackBox() => ToggleAttack(true);
        public void OffAttackBox() => ToggleAttack(false);
        #endregion

        #region 돌진 공격
        private bool DetectObstacle(string tag, out RaycastHit hitInfo)
        {
            Vector3 directionToWall = transform.forward;
            float detectionRange = 1.5f;
            return Physics.Raycast(transform.position, directionToWall, out hitInfo, detectionRange) && hitInfo.transform.CompareTag(tag);
        }

        public void TriggerRushWarning()
        {
            if (canSpawn)
            {
                StartCoroutine(ShowRushWarning());
            }
        }

        private IEnumerator ShowRushWarning()
        {
            Vector3 startPosition = transform.position; // 보스의 현재 위치
            Vector3 rushDirection = transform.forward; // 돌진 방향 (현재 보스의 정면 방향)

            // 경고 이펙트를 돌진 방향으로 길게 생성
            Vector3 warningPosition = startPosition + rushDirection * (warningLength / 2f);
            Quaternion warningRotation = Quaternion.LookRotation(rushDirection);

            // 경고 이펙트 생성
            GameObject warningEffect = Instantiate(warningEffectPrefab, startPosition, warningRotation, this.transform);
            warningEffect.transform.GetChild(0).localScale = new Vector3(1, 0.1f, warningLength); // 길이 조정

            canSpawn = false;

            // 경고 지속 시간 동안 계속 체크하여 벽이 감지되면 즉시 중단
            float elapsedTime = 0f;
            while (elapsedTime < warningDuration)
            {
                if (animator.GetBool("IsWall") || animator.GetBool("IsBerserk"))
                {
                    Destroy(warningEffect); // 기존 경고 이펙트 제거
                    yield break; // 즉시 종료 (return 역할)
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Destroy(warningEffect); // 경고 표시 제거 후 돌진 시작 

            yield return new WaitForSeconds(Duration);

            startPosition = transform.position; // 보스의 현재 위치
            rushDirection = transform.forward; // 돌진 방향 (현재 보스의 정면 방향)

            warningRotation = Quaternion.LookRotation(rushDirection);

            GameObject secondWarningEffect = Instantiate(warningEffectPrefab, startPosition, warningRotation, this.transform);
            secondWarningEffect.transform.GetChild(0).localScale = new Vector3(1, 0.1f, warningLength); // 길이 조정

            StartCoroutine(ResetDamageCooldown());

            elapsedTime = 0f;
            while (elapsedTime < secondWarningDuration)
            {
                if (animator.GetBool("IsWall") || animator.GetBool("IsBerserk"))
                {
                    Destroy(secondWarningEffect); // 두 번째 경고 이펙트 제거
                    yield break; // 즉시 종료
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Destroy(secondWarningEffect);
        }


        private IEnumerator ResetDamageCooldown()
        {
            yield return new WaitForSeconds(warningDuration);
            canSpawn = true;
        }

        private bool DetectWall() => DetectObstacle("Wall", out _);

        private bool DetectBWall()
        {
            if (DetectObstacle("BWall", out RaycastHit hit))
            {
                hit.transform.GetComponent<BrokenWall>()?.RushToWall();
                return true;
            }
            return false;
        }
        #endregion
    }
}
