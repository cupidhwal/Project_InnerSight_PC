using UnityEngine;

namespace JungBin
{

    public class BossStageManager : MonoBehaviour
    {
        [SerializeField] private BossHealthUI bossHealthUI; // 체력 UI 스크립트
        [SerializeField] private BossStat[] bosses; // 스테이지에 있는 모든 보스

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

                Debug.Log($"스테이지에 {selectedBoss.name} 보스가 활성화되었습니다.");
            }
            else
            {
                Debug.LogError("잘못된 보스 인덱스입니다.");
            }
        }
    }
}