using UnityEngine;

namespace JungBin
{

    public class TestUI : MonoBehaviour
    {
        [SerializeField] private Canvas pauseMenuCanvas; // Inspector에서 등록할 UI 캔버스
        private bool isGamePaused = false; // 게임이 멈춘 상태인지 확인

        [SerializeField] private BossStageManager bossStageManager;

        private void Start()
        {
            // UI 캔버스를 비활성화하여 시작 시 보이지 않게 설정
            if (pauseMenuCanvas != null)
            {
                pauseMenuCanvas.gameObject.SetActive(false);

            }
            bossStageManager.EnterBossStage(0);

        }

        private void Update()
        {
            // Y 키를 눌렀을 때 게임을 일시정지하거나 재개
            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (isGamePaused)
                {
                    ResumeGame(); // 게임 재개
                }
                else
                {
                    PauseGame(); // 게임 일시정지
                }
            }
        }

        // 게임 일시정지
        public void PauseGame()
        {
            if (pauseMenuCanvas != null)
            {
                pauseMenuCanvas.gameObject.SetActive(true); // UI 캔버스 활성화
            }

            Time.timeScale = 0f; // 게임 시간 멈춤
            isGamePaused = true; // 상태 업데이트
            Cursor.lockState = CursorLockMode.None; // 마우스 커서 잠금 해제
            Cursor.visible = true; // 마우스 커서 보이게 설정
        }

        // 게임 재개
        public void ResumeGame()
        {
            if (pauseMenuCanvas != null)
            {
                pauseMenuCanvas.gameObject.SetActive(false); // UI 캔버스 비활성화
            }

            Time.timeScale = 1f; // 게임 시간 정상화
            isGamePaused = false; // 상태 업데이트
            Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 잠금
            Cursor.visible = false; // 마우스 커서 숨김
        }

        public void CloseUI()
        {
            isGamePaused = false;
        }
    }
}