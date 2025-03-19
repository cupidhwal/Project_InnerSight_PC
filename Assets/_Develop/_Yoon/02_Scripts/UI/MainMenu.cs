using UnityEngine;
using UnityEngine.UI;
using Noah;
using InnerSight_Kys;

namespace Yoon
{
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        SceneFade fader;
        [SerializeField] private string loadToScene = "PlayScene";        //playScene 불러오기
        [SerializeField] private string loadToTestScene = "";             //TestScene 불러오기
        [SerializeField] private GameObject contiune;

        public GameObject mainMenuUI;
        public GameObject newGameWarning;
        public GameObject creditUI;

        public Animator creditAnim;

        //public GameObject continueButton;
        SaveLoadManager saveLoadManager;

        #endregion

        private void Start()
        {
            //게임 데이터 초기화
            InitGameData();
        }

        private void InitGameData()
        {
            fader = SceneFade.instance;
            saveLoadManager = SaveLoadManager.Instance;
            saveLoadManager.EmptyData(contiune);

            AudioManager.Instance.PlayBgm("MainScene");
        }
        private void PlayButtonClickSound()
        {
            AudioManager.Instance.Play("Button Click Sound");
        }

        public void NewGame()
        {
            newGameWarning.SetActive(true);
        }

        public void NewGameStart()
        {
            PlayButtonClickSound();

            //게임 데이터 초기화
            saveLoadManager.DeleteAllSaveFiles();

            fader.FadeOut(loadToScene);
        }
        public void NewGamePopUpClose()
        {
            newGameWarning.SetActive(false);
        }

        public void LoadGame()
        {
            PlayButtonClickSound();
            Debug.Log("LoadGame");

            fader.FadeOut(loadToScene);
        }


        public void Credits()
        {
            PlayButtonClickSound();
            ShowCredit();
        }

        //크레딧 UI 실행
        private void ShowCredit()
        {
            Debug.Log("ShowCredit");

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

        public void TestPlay()
        {
            PlayButtonClickSound();

            //게임 데이터 초기화
            saveLoadManager.DeleteAllSaveFiles();

            fader.FadeOut(loadToTestScene);
        }

        public void QuitGame()
        {
            PlayButtonClickSound();
            Debug.Log("Quit Game");
            Application.Quit();
        }
    }
}