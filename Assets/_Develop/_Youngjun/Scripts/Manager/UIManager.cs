using TMPro;
using UnityEngine;

namespace Noah
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject skillSelectUI;
        public GameObject playerStateUI;
        public TMP_Text goldText;

        InGameUI_PlayerStats inGameUI_PlayerState;

        public void Init()
        {
            inGameUI_PlayerState = playerStateUI.transform.parent.GetComponent<InGameUI_PlayerStats>();

            UpdateGoldUI();
        }

        public void UpdateGoldUI()
        {
            if(goldText != null)
            {
                goldText.text = PlayerInfoManager.Instance.GetGold().ToString();
                inGameUI_PlayerState.gold_Text.text = PlayerInfoManager.Instance.GetGold().ToString();
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
