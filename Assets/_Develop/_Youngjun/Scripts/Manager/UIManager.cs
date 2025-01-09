using TMPro;
using UnityEngine;

namespace Noah
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject skillSelectUI;
        public TMP_Text goldText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            UpdateGoldUI();
        }

        public void UpdateGoldUI()
        {
            goldText.text = PlayerInfoManager.Instance.Gold.ToString();
        }
    }

}
