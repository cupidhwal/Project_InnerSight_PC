using Seti;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Noah
{
    public class InGameUI_PlayerStats : MonoBehaviour
    {
        public TMP_Text gold_Text;

        private GameObject player;
        private Transform stateGroup;
        public List<Transform> states = new List<Transform>();
        private List<float> currentDatas = new List<float>();

        private int[] upgradeCounts;

        private float curData;
        private float currentPoint;

        public float maxData_Speed = 13f;

        public void Init()
        {
            player = FindAnyObjectByType<RayManager>().gameObject;

            stateGroup = transform.GetChild(0).GetChild(0);

            for (int i = 0; i < stateGroup.childCount; i++)
            {
                states.Add(stateGroup.GetChild(i));
            }

            upgradeCounts = new int[PlayerStatsManager.Instance.UpgardeCount().Count];
        }

        public void GetStateData()
        {
            player.GetComponent<Condition_Player>().PlayerSetActive(false);
            player.GetComponent<PlayerUseSkill>().enabled = false;

            for (int i = 0; i < states.Count; i++)
            {
                upgradeCounts[i] = PlayerStatsManager.Instance.UpgardeCount()[i];

                states[i].GetChild(0).GetComponent<TMP_Text>().text = 
                    PlayerStatsManager.Instance.GetPlayerData()[i].ToString();

                states[i].GetChild(4).GetComponent<TMP_Text>().text =
                      (upgradeCounts[i] * PlayerStatsManager.Instance.GetUpgradeCost()[i]).ToString();
            }

            gold_Text.text = PlayerInfoManager.Instance.GetGold().ToString();
        }

        void LimitState(int _index, ref float amount)
        {
            if (_index == 3 || _index == 4) // MoveSpeed 또는 AttackSpeed
            {
                if (amount > maxData_Speed)
                {
                    amount = maxData_Speed; // maxData_Speed 이상으로 못 올라가게 제한
                } 
            }

        }

        public void AddButton(int _index)
        {
            int cost = upgradeCounts[_index] * PlayerStatsManager.Instance.GetUpgradeCost()[_index];

            int totalGold = int.Parse(gold_Text.text) - cost;

            if (totalGold >= 0)
            {
                if(states[_index].GetChild(0).GetComponent<TMP_Text>().text == "MAX")
                {
                    return;
                }

                float upPoint = Mathf.Round((float.Parse(states[_index].GetChild(0).GetComponent<TMP_Text>().text)
                                                + PlayerStatsManager.Instance.UpdatePlayerData()[_index]) * 10f) / 10f;

                //LimitState(_index, ref upPoint);

                // upPoint가 maxData_Speed으로 수정되었을 경우 return하여 실행 중단
                if ((_index == 3 || _index == 4) && upPoint >= maxData_Speed)
                {
                    states[_index].GetChild(0).GetComponent<TMP_Text>().text = "MAX";
                    //return;
                }
                else
                {
                    states[_index].GetChild(0).GetComponent<TMP_Text>().text = upPoint.ToString();
                }

                upgradeCounts[_index]++;

                cost = upgradeCounts[_index] * PlayerStatsManager.Instance.GetUpgradeCost()[_index];

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
            curData = PlayerStatsManager.Instance.GetPlayerData(_index);

            if (states[_index].GetChild(0).GetComponent<TMP_Text>().text == "MAX")
            {
                currentPoint = maxData_Speed;
            }
            else
            {
                currentPoint = float.Parse(states[_index].GetChild(0).GetComponent<TMP_Text>().text);
            }

         

            // 상태값 감소 계산
            float downPoint = Mathf.Round((currentPoint - PlayerStatsManager.Instance.UpdatePlayerData()[_index]) * 10f) / 10f;

            // 초기 상태 이하로 감소 불가
            if (downPoint < curData && !Mathf.Approximately(downPoint, curData))
            {
                Debug.Log("더 내려갈 수 없음");
                return;
            }

            // 상태값 감소
            states[_index].GetChild(0).GetComponent<TMP_Text>().text = downPoint.ToString();

            // 골드 복구 (현재 단계의 비용 복구)
            int currentUpgradeCost = (upgradeCounts[_index] - 1) * PlayerStatsManager.Instance.GetUpgradeCost()[_index];
            int totalGold = int.Parse(gold_Text.text) + currentUpgradeCost;

            // 업그레이드 횟수 감소
            upgradeCounts[_index]--;

            // 새로운 비용 계산 (다음 단계 비용)
            int nextUpgradeCost = upgradeCounts[_index] * PlayerStatsManager.Instance.GetUpgradeCost()[_index];
            states[_index].GetChild(4).GetComponent<TMP_Text>().text = nextUpgradeCost.ToString();

            // 골드 텍스트 업데이트
            gold_Text.text = totalGold.ToString();

            Debug.Log($"골드 복구: {currentUpgradeCost}, 최종 골드: {totalGold}");
        }

        public void ApplyState()
        {
            PlayerStatsManager.Instance.UpdateStateData(states, upgradeCounts);
            PlayerInfoManager.Instance.SetGold(int.Parse(gold_Text.text));
            UIManager.Instance.playerStateUI.gameObject.SetActive(false);
            player.GetComponent<Condition_Player>().PlayerSetActive(true);

            SaveLoadManager.Instance.SaveAll();
        }
        
        public void ActiveUI()
        {
            player.GetComponent<Condition_Player>().PlayerSetActive(true);
            player.GetComponent<PlayerUseSkill>().enabled = true;

            UIManager.Instance.Toggle(transform.GetChild(0).gameObject);


        }
    }
}
