using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Noah
{
    public class InGameUI_PlayerState : MonoBehaviour
    {
        public TMP_Text gold_Text;

        private Transform stateGroup;
        public List<Transform> states = new List<Transform>();
        private List<float> currentDatas = new List<float>();

        private float curData;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void Init()
        {
            stateGroup = transform.GetChild(0).GetChild(0);

            for (int i = 0; i < stateGroup.childCount; i++)
            {
                states.Add(stateGroup.GetChild(i));
            }
        }

        public void GetStateData()
        {
            for (int i = 0; i < states.Count; i++)
            {
                states[i].GetChild(0).GetComponent<TMP_Text>().text = 
                    PlayerStateManager.Instance.GetPlayerData()[i].ToString();          
            }

            gold_Text.text = PlayerInfoManager.Instance.Gold.ToString();

            curData = float.Parse(states[0].GetChild(0).GetComponent<TMP_Text>().text);
        }

        public void AddButton(int _index)
        {
            float upPoint = float.Parse(states[_index].GetChild(0).GetComponent<TMP_Text>().text) + PlayerStateManager.Instance.UpdatePlayerData()[_index];

            states[_index].GetChild(0).GetComponent<TMP_Text>().text = upPoint.ToString();
        }
        
        public void RemoveButton(int _index)
        {
            float downPoint = float.Parse(states[_index].GetChild(0).GetComponent<TMP_Text>().text) - PlayerStateManager.Instance.UpdatePlayerData()[_index];

            if (downPoint <= curData)
            {

            }
            else
            {
                states[_index].GetChild(0).GetComponent<TMP_Text>().text = downPoint.ToString();
            }
        }

        public void ApplyState()
        {
            PlayerStateManager.Instance.UpdateStateData(states);
            UIManager.Instance.playerStateUI.gameObject.SetActive(false);
        }

    }
}
