using UnityEngine;
using UnityEngine.AI;

namespace JungBin
{

    public class SecondBossManager : MonoBehaviour
    {
        #region Variables

        private int lastAttack = -1;
        public static bool isAttack { get; set; } = false; // 공격중인지 여부

        [SerializeField] private Transform player;
        [SerializeField] private Animator animator;
        private NavMeshAgent navMeshAgent;
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

        }
    }
}