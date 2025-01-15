using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Noah
{
    public class InGameUI_PlayerState : MonoBehaviour
    {
        public TMP_Text gold_Text;

        private Transform stateGroup;
        private List<Transform> states = new List<Transform>();


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

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

        }

    }
}
