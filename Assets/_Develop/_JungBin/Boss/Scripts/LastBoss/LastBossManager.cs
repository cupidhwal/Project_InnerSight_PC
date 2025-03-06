using UnityEngine;
using UnityEngine.AI;

namespace JungBin
{
    public class LastBossManager : MonoBehaviour
    {
        #region Variables

        [Header("General Settings")]
        [SerializeField] private int bossAttackNumber = 0;
        [SerializeField] private float turnSpeed = 30; // 보스의 회전 속도
        [SerializeField] private float detectionRange = 8f;

        private int lastAttack = -1;
        public static bool isAttack { get; set; } = false;

        [SerializeField] private Transform player;
        [SerializeField] private Animator animator;
        private NavMeshAgent navMeshAgent;

        private string Idle = "Idle";
        private string isFar = "IsFar";
        private string isRun = "IsRun";
        private string isArrived = "IsArrived";

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

            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            if (animator.GetBool("IsDeath") || player == null) return;

            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            if (!isAttack) RotateTowardsPlayer(direction);

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

        public void SelectNextAttack()  //보스의 공격 패턴 결정(연속으로 같은 공격이 나오지는 않음)
        {


            int attackIndex;
            do
            {
                attackIndex = Random.Range(1, bossAttackNumber);
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
    }
}