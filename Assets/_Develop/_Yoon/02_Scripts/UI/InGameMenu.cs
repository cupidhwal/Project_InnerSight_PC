using Noah;
using UnityEngine;

namespace Yoon
{

public class InGameMenu : MonoBehaviour
{
        #region Variables
        private SceneFade fader;                                                //fader 불러오기
        private MainMenu mainMenu;                                              //메인메뉴
        [SerializeField] private string loadToScene = "MainMenu";        //playScene 불러오기

        public GameObject menuUI;                                       //in game ui
        private bool isMenuActive = false; // 메뉴 활성화 여부 체크

        #endregion
        private void Start()
        {
            //게임 데이터 초기화
            InitGameData();
            menuUI.SetActive(false);

            if (mainMenu == null)
            {
                mainMenu = FindObjectOfType<MainMenu>();
                if (mainMenu == null)
                {
                    Debug.LogWarning("MainMenu 객체를 찾을 수 없습니다.");
                }
            }
        }

        private void InitGameData()
        {
            fader = SceneFade.instance;
        }

        // Update is called once per frame
        void Update()
    {
            // ESC 키가 눌리면 크레딧 종료
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }
        private void ToggleMenu()
        {
            isMenuActive = !isMenuActive; // 상태 변경
            menuUI.SetActive(isMenuActive);

            // 메뉴가 활성화되면 게임을 멈추고, 비활성화되면 다시 실행
            Time.timeScale = isMenuActive ? 0f : 1f;
        }

        public void Continue()
        {
            ToggleMenu();
        }
        public void BackToMenu()
        {
            Debug.Log("LoadGame");
            Time.timeScale = 1f; // 씬 이동 전 게임 속도 복구
            fader.FadeOut(loadToScene);
        }
        public void QuitGame()
        {
            Time.timeScale = 1f; // 게임 종료 전 속도 복구
            Debug.Log("Quit Game");
            Application.Quit();
        }
    }


}