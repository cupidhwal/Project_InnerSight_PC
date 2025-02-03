using TMPro;
using UnityEngine;

namespace Noah
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject skillSelectUI;
        public GameObject playerStateUI;
        public TMP_Text goldText;

        InGameUI_PlayerState inGameUI_PlayerState;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Init();
            UpdateGoldUI();
        }

        void Init()
        {
            inGameUI_PlayerState = playerStateUI.transform.parent.GetComponent<InGameUI_PlayerState>();
        }

        public void UpdateGoldUI()
        {
            if(goldText != null)
            {
                goldText.text = PlayerInfoManager.Instance.Gold.ToString();
                inGameUI_PlayerState.gold_Text.text = PlayerInfoManager.Instance.Gold.ToString();
            }         
        }

        public void ActivePlayerStateUI()
        {
            if (!playerStateUI.activeSelf)
            {
                playerStateUI.SetActive(true);
                inGameUI_PlayerState.GetStateData();
            }
            else
            {
                inGameUI_PlayerState.ActiveUI();
            }
        }

        public void Toggle(GameObject _ui)
        {
            _ui.SetActive(!_ui);
        }
    }

}
