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

        [SerializeField] private Transform slashSpawnPoint; 
        [SerializeField] private GameObject slashAttack;    //보스 공격 이펙트(슬레쉬)

        [SerializeField] private Transform shockSpawnPoint;
        [SerializeField] private GameObject shockAttack;    //보스 공격 이펙트(충격)

        [SerializeField] private Transform fireSpawnPoint;
        [SerializeField] private ParticleSystem fireAttack;    //보스 공격 이펙트(충격)

        [SerializeField] private BoxCollider smashAttackBox;
        [SerializeField] private FlameAttack flameAttack;

        #endregion

        private void Start()
        {
            ResetBoss();

            
        }

        // Update is called once per frame
        void Update()
        {
            if (animator.GetBool("IsDeath") || player == null) return;

            if (animator.GetBool("Start") == false) return;

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

            if(distance <= 15)
            {
                animator.SetTrigger("Start");
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

            OneHandSword.SetActive(true);
            TwoHandSword.SetActive(false);
            smashAttackBox.enabled = false;

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
/*        public void WeaponSelect()
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
        }*/

        public void ChangeWeapon()
        {
            OneHandSword.SetActive(false);
            TwoHandSword.SetActive(true);

        }

        #endregion

        #region 공격 상태

        public void ToggleAttack(bool isActive)
        {
            smashAttackBox.enabled = isActive;
        }

        public void OnAttackBox() => ToggleAttack(true);
        public void OffAttackBox() => ToggleAttack(false);

        public void SlashAttack()
        {
            GameObject slashParticle = Instantiate(slashAttack, slashSpawnPoint.position, slashSpawnPoint.rotation);

            Destroy(slashParticle, 1.5f);
        }

        public void ShockAttack()
        {
            Vector3 SpawnPoint = shockSpawnPoint.position;
            SpawnPoint.y = 0;

            GameObject shockParticle = Instantiate(shockAttack, SpawnPoint, shockSpawnPoint.rotation);

            Destroy(shockParticle, 1f);
        }

        public void FireAttack()
        {
            fireAttack.Play();
            flameAttack.StartFlamethrower();
        }

        public void FireOffAttack()
        {
            fireAttack.Stop();
            flameAttack.StopFlamethrower();
        }

        #endregion
    }
}