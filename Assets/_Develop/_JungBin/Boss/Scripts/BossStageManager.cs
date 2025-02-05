using UnityEngine;

namespace JungBin
{

    public class BossStageManager : MonoBehaviour
    {
        public static BossStageManager Instance { get; private set; } // 싱글톤 인스턴스

        [SerializeField] private GameObject player;
        [SerializeField] private BossHealthUI bossHealthUI; // 체력 UI 스크립트
        [SerializeField] private BossStat[] bosses; // 스테이지에 있는 모든 보스

        public GameObject Player => player; // 외부에서 접근 가능한 프로퍼티
        public BossStat[] Bosses => bosses; 

        private void Awake()
        {
            // 싱글톤 초기화
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogError("Player GameObject with tag 'Player' not found! Make sure the GameObject is tagged as 'Player'.");
            }
        }

        
        public void EnterBossStage(int bossIndex)
        {
            if (bossIndex >= 0 && bossIndex < bosses.Length)
            {
                BossStat selectedBoss = bosses[bossIndex];

                // 선택된 보스 활성화
                foreach (var boss in bosses)
                {
                    boss.gameObject.SetActive(boss == selectedBoss);
                }

                // 보스 체력 UI 연결
                bossHealthUI.SetBoss(selectedBoss);

            }
            else
            {
                Debug.LogError("잘못된 보스 인덱스입니다.");
            }
        }
    }
}