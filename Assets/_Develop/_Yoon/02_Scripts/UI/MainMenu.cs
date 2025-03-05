using UnityEngine;
using UnityEngine.UI;
using Noah;

namespace Yoon
{
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        SceneFade fader;
        [SerializeField] private string loadToScene = "PlayScene";        //playScene 불러오기
        [SerializeField] private GameObject contiune;

        public GameObject mainMenuUI;
        public GameObject creditUI;

        public Animator creditAnim;

        //public GameObject continueButton;
        SaveLoadManager saveLoadManager;

        #endregion

        private void Start()
        {
            //게임 데이터 초기화
            InitGameData();
            
            //저장된 씬이 있으면

            //if(PlayerStats.Instance.SceneNumber > 0)
            //{
            //    continueButton.SetActive(true);
            //}

            //씬 페이드인 효과
            //fader.FromFade();                 //TODO : fader 스크립트 확인


        }

        private void InitGameData()
        {
            fader = SceneFade.instance;
            saveLoadManager = SaveLoadManager.Instance;
            saveLoadManager.EmptyData(contiune);

            //게임 플레이 데이터 로드
            //PlayData playData = SaveLoad.LoadData();
            //PlayerStats.Instance.PlayerStatInit(playData);
        }

        public void NewGame()
        {
            Debug.Log("NewGame");


            //게임 데이터 초기화
            saveLoadManager.DeleteAllSaveFiles();
            //PlayerStats.Instance.PlayerStatInit(null);        //

            fader.FadeOut(loadToScene);
        }

        public void LoadGame()
        {
            Debug.Log("LoadGame");

            fader.FadeOut(loadToScene);
        }


        public void Credits()
        {
            ShowCredit();
        }

        //크레딧 UI 실행
        private void ShowCredit()
        {
            Debug.Log("ShowCredit");            //

            mainMenuUI.SetActive(false);
            creditUI.SetActive(true);

            creditAnim.Play("CreditAnimation");
        }

        public void OnCreditEnd()
        {
            Debug.Log("Credit End");
            creditUI.SetActive(false);
            mainMenuUI.SetActive(true);
        }


        public void QuitGame()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }
    }
}