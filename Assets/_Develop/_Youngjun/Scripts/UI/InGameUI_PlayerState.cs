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
        private int curUpdateCount;
        private int totalCost;

        private int upgardeCount;
        private int upgardeCost;

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

                states[i].GetChild(4).GetComponent<TMP_Text>().text =
                    PlayerStateManager.Instance.GetUpgradeCost()[i].ToString();
            }

            gold_Text.text = PlayerInfoManager.Instance.Gold.ToString();
        }

        public void AddButton(int _index)
        {
            int cost = PlayerStateManager.Instance.UpgardeCount()[_index] * PlayerStateManager.Instance.GetUpgradeCost()[_index];

            int totalGold = int.Parse(gold_Text.text) - cost;

            if (totalGold >= 0)
            {
                float upPoint = float.Parse(states[_index].GetChild(0).GetComponent<TMP_Text>().text) + PlayerStateManager.Instance.UpdatePlayerData()[_index];

                states[_index].GetChild(0).GetComponent<TMP_Text>().text = upPoint.ToString();

                PlayerStateManager.Instance.UpgardeCount()[_index]++;

                Debug.Log(PlayerStateManager.Instance.UpgardeCount()[_index]);

                cost = PlayerStateManager.Instance.UpgardeCount()[_index] * PlayerStateManager.Instance.GetUpgradeCost()[_index];

                states[_index].GetChild(4).GetComponent<TMP_Text>().text = cost.ToString();

                gold_Text.text = totalGold.ToString();


            }


        }
        
        public void RemoveButton(int _index)
        {
            curData = PlayerStateManager.Instance.GetPlayerData(_index);

            float downPoint = float.Parse(states[_index].GetChild(0).GetComponent<TMP_Text>().text) - PlayerStateManager.Instance.UpdatePlayerData()[_index];

            if (downPoint < curData)
            {
                Debug.Log("더 내려갈 수 없음");
            }
            else
            {
                states[_index].GetChild(0).GetComponent<TMP_Text>().text = downPoint.ToString();
            }
        }

        public void ApplyState()
        {
            PlayerStateManager.Instance.UpdateStateData(states);
            PlayerInfoManager.Instance.UseGold(totalCost);
            UIManager.Instance.playerStateUI.gameObject.SetActive(false);
        }
        
        public void ActiveUI()
        {
            UIManager.Instance.Toggle(transform.GetChild(0).gameObject);
        }
    }
}
