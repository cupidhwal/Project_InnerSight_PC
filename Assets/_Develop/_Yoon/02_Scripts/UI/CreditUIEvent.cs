using UnityEngine;

namespace Yoon
{

    public class CreditUIEvent : MonoBehaviour
    {
        public MainMenu SceneManagerMainMenu; // SceneManager의 MainMenu 스크립트 참조

        private void Update()
        {
            // ESC 키가 눌리면 크레딧 종료
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CreditEventEnd();
            }
        }

        public void CreditEventEnd()
        {

            SceneManagerMainMenu.OnCreditEnd(); // SceneManager의 OnCreditEnd() 호출

        }
    }

}