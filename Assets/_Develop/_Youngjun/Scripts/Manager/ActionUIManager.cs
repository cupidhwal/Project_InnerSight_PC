using TMPro;
using UnityEngine;

namespace Noah
{
    public class ActionUIManager : Singleton<ActionUIManager>
    {
        [SerializeField] private Transform actionUI;

        private TMP_Text actionUI_Text;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            actionUI_Text = actionUI.GetChild(1).GetComponent<TMP_Text>();
        }

        public void EnableActionUI(string _text)
        {
            actionUI.gameObject.SetActive(true);

            actionUI_Text.text = _text;
        }

        public void DisableActionUI()
        {
            actionUI.gameObject.SetActive(false);
        }
    }
}