using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JungBin
{

    public class RelicManager : MonoBehaviour
    {
        private List<IRelic> relics = new List<IRelic>();
        private IRelic selectedRelic; // 마지막으로 선택한 유물 저장

        [SerializeField] private TextMeshProUGUI relicName;      //유물의 이름을 나타내는 텍스트 오브젝트
        [SerializeField] private TextMeshProUGUI relicDescription;      //유물 설명을 해줄 텍스트 오브젝트
        [SerializeField] private GameObject relicSelectUI;

        // 유물 추가
        public void AddRelic(IRelic relic, Player player)
        {
            relics.Add(relic);
            Debug.Log($"{relic.RelicName} 유물의 효과 : {relic.Description}");
        }

        // 현재 유물 목록 반환
        public List<IRelic> GetRelics()
        {
            return relics;
        }

        // 현재 적용 중인 유물이 있는지 확인
        public bool HasActiveRelic()
        {
            return selectedRelic != null;
        }

        public void ClickRelicButton(string name)
        {
            ShowRelicDescription(name);
        }

        public void SelectRelicButton()
        {
            ApplyRelicEffect(selectedRelic, GameManager.Instance.Player);
        }

        public IRelic ShowRelicDescription(string name)
        {
            Debug.Log("클릭");
            if (relicDescription != null)
            {
                foreach (var relic in GetRelics())
                {
                    if (name == relic.RelicName)
                    {
                        relicDescription.gameObject.SetActive(true);
                        relicName.text = relic.RelicName;
                        relicDescription.text = relic.Description;

                        selectedRelic = relic; // 선택된 유물 저장
                        return relic;
                    }
                }
            }
            Debug.LogWarning("선택한 유물이 없습니다.");
            return null;
        }

        // 유물 효과 적용 (새 유물 선택 시 이전 효과 제거)
        public void ApplyRelicEffect(IRelic newRelic, Player player)
        {
            // 이전 유물 효과 제거
            if (selectedRelic != null)
            {
                RemoveRelicEffect(selectedRelic, player);
            }

            // 새 유물 효과 적용
            selectedRelic = newRelic;
            selectedRelic.ApplyEffect(player);
            Debug.Log($"{selectedRelic.RelicName} 유물 효과가 적용되었습니다.");

            relicSelectUI.SetActive(false);
        }

        // 이전 유물 효과 제거
        private void RemoveRelicEffect(IRelic relic, Player player)
        {
            relic.RemoveEffect(player);
            Debug.Log($"{relic.RelicName} 유물 효과가 제거되었습니다.");
        }
    }
}