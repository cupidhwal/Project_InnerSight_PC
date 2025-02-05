using Seti;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Noah
{
    public class InGameUI_PlayerState : MonoBehaviour
    {
        public TMP_Text gold_Text;

        public GameObject player;
        private Transform stateGroup;
        public List<Transform> states = new List<Transform>();
        private List<float> currentDatas = new List<float>();

        private int[] upgradeCounts;

        private float curData;
        private int totalCost;
        private int totalGold;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public void Init()
        {
            player = FindAnyObjectByType<RayManager>().gameObject;

            stateGroup = transform.GetChild(0).GetChild(0);

            for (int i = 0; i < stateGroup.childCount; i++)
            {
                states.Add(stateGroup.GetChild(i));
            }

            upgradeCounts = new int[PlayerStateManager.Instance.UpgardeCount().Count];
        }

        public void GetStateData()
        {
            player.GetComponent<Condition_Player>().PlayerSetActive(false);

            for (int i = 0; i < states.Count; i++)
            {
                upgradeCounts[i] = PlayerStateManager.Instance.UpgardeCount()[i];

                states[i].GetChild(0).GetComponent<TMP_Text>().text = 
                    PlayerStateManager.Instance.GetPlayerData()[i].ToString();

                states[i].GetChild(4).GetComponent<TMP_Text>().text =
                      (upgradeCounts[i] * PlayerStateManager.Instance.GetUpgradeCost()[i]).ToString();
            }

            gold_Text.text = PlayerInfoManager.Instance.Gold.ToString();
        }

        public void AddButton(int _index)
        {
            int cost = upgradeCounts[_index] * PlayerStateManager.Instance.GetUpgradeCost()[_index];

            int totalGold = int.Parse(gold_Text.text) - cost;

            if (totalGold >= 0)
            {
                float upPoint = float.Parse(states[_index].GetChild(0).GetComponent<TMP_Text>().text) + PlayerStateManager.Instance.UpdatePlayerData()[_index];

                states[_index].GetChild(0).GetComponent<TMP_Text>().text = upPoint.ToString();

                upgradeCounts[_index]++;

                cost = upgradeCounts[_index] * PlayerStateManager.Instance.GetUpgradeCost()[_index];

                states[_index].GetChild(4).GetComponent<TMP_Text>().text = cost.ToString();

                gold_Text.text = totalGold.ToString();


            }


        }
        
        public void RemoveButton(int _index)
        {
            // 업그레이드가 0인 경우 더 이상 제거 불가
            if (upgradeCounts[_index] <= 0)
            {
                Debug.Log("더 내려갈 수 없음");
                return;
            }

            // 현재 상태값과 초기 상태값 가져오기
            curData = PlayerStateManager.Instance.GetPlayerData(_index);
            float currentPoint = float.Parse(states[_index].GetChild(0).GetComponent<TMP_Text>().text);

            // 상태값 감소 계산
            float downPoint = currentPoint - PlayerStateManager.Instance.UpdatePlayerData()[_index];

            // 초기 상태 이하로 감소 불가
            if (downPoint < curData && !Mathf.Approximately(downPoint, curData))
            {
                Debug.Log("더 내려갈 수 없음");
                return;
            }

            // 상태값 감소
            states[_index].GetChild(0).GetComponent<TMP_Text>().text = downPoint.ToString();

            // 골드 복구 (현재 단계의 비용 복구)
            int currentUpgradeCost = (upgradeCounts[_index] - 1) * PlayerStateManager.Instance.GetUpgradeCost()[_index];
            int totalGold = int.Parse(gold_Text.text) + currentUpgradeCost;

            // 업그레이드 횟수 감소
            upgradeCounts[_index]--;

            // 새로운 비용 계산 (다음 단계 비용)
            int nextUpgradeCost = upgradeCounts[_index] * PlayerStateManager.Instance.GetUpgradeCost()[_index];
            states[_index].GetChild(4).GetComponent<TMP_Text>().text = nextUpgradeCost.ToString();

            // 골드 텍스트 업데이트
            gold_Text.text = totalGold.ToString();

            Debug.Log($"골드 복구: {currentUpgradeCost}, 최종 골드: {totalGold}");
        }

        public void ApplyState()
        {
            PlayerStateManager.Instance.UpdateStateData(states, upgradeCounts);
            PlayerInfoManager.Instance.SetGold(int.Parse(gold_Text.text));
            UIManager.Instance.playerStateUI.gameObject.SetActive(false);
            player.GetComponent<Condition_Player>().PlayerSetActive(true);
        }
        
        public void ActiveUI()
        {
            player.GetComponent<Condition_Player>().PlayerSetActive(true);

            UIManager.Instance.Toggle(transform.GetChild(0).gameObject);


        }
    }
}
