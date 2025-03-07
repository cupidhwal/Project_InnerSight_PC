using System.Collections;
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
        private string isRun = "IsRun";


        [Header("LastBoss Settings")]
        [SerializeField] private GameObject OneHandSword;
        [SerializeField] private GameObject TwoHandSword;
        #endregion

        private void Start()
        {
            ResetBoss();

            
        }

        // Update is called once per frame
        void Update()
        {
            if (animator.GetBool("IsDeath") || player == null) return;

            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            animator.SetFloat("PlayerDistance", distance);

            if (!isAttack) RotateTowardsPlayer(direction);

            if (animator.GetBool(isRun))
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

        private void ResetBoss()
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

            navMeshAgent = GetComponent<NavMeshAgent>();

            OneHandSword.SetActive(false);
            TwoHandSword.SetActive(false);
        }

        public void StartWeaponSelect()
        {
            // 랜덤으로 OneHandSword 또는 TwoHandSword 중 하나만 활성화
            bool isOneHand = Random.value > 0.5f;

            // Root 애니메이터에도 반영
            OneHandSword.SetActive(isOneHand);
            TwoHandSword.SetActive(!isOneHand);

            // 보스 애니메이션도 맞춰서 시작
            animator.SetTrigger(isOneHand ? "OneHandWeapon" : "TwoHandWeapon");
        }

        private void RotateTowardsPlayer(Vector3 direction)
        {
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
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

            Vector3 pos = transform.position;
            pos.y = 0f; // Y값을 0으로 고정
            transform.position = pos;
        }

        private void TriggerAttackAnimation(int attackIndex)    // 결정된 공격 패턴을 애니메이션에게 전달
        {
            animator.SetTrigger($"Attack0{attackIndex}");
            animator.SetBool(Idle, false);
        }
        public void WeaponSelect()
        {
            bool isOneHandActive = OneHandSword.activeSelf;
            bool isTwoHandActive = TwoHandSword.activeSelf;

            if (isOneHandActive && !isTwoHandActive)
            {
                // 한손검 → 양손검으로 변경
                OneHandSword.SetActive(false);
                TwoHandSword.SetActive(true);
                animator.SetTrigger("TwoHandWeapon");
            }
            else if (isTwoHandActive && !isOneHandActive)
            {
                // 양손검 → 한손검으로 변경
                OneHandSword.SetActive(true);
                TwoHandSword.SetActive(false);
                animator.SetTrigger("OneHandWeapon");
            }
        }

        public void ChangeWeapon()
        {
            if (Random.value < 0.2f) 
            {
                animator.SetTrigger("WeaponChange");
            }
        }

        #endregion
    }
}