using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JungBin
{

    public class RelicManager : MonoBehaviour
    {
        private List<IRelic> relics = new List<IRelic>();
        [SerializeField] private TextMeshProUGUI relicDescription;      //유물 설명을 해줄 텍스트 오브젝트

        // 유물 추가
        public void AddRelic(IRelic relic, Player player)
        {
            relics.Add(relic);
            relic.ApplyEffect(player);
            Debug.Log($"{relic.RelicName} 유물의 효과 : {relic.Description}");
        }

        // 현재 유물 목록 반환
        public List<IRelic> GetRelics()
        {
            return relics;
        }

        public void ShowRelicDescription(string name)
        {
            if (relicDescription != null)
            {
                for (int i = 0; i < GetRelics().Count; i++)
                {
                    if (name == GetRelics()[i].RelicName)
                    {
                        relicDescription.gameObject.SetActive(true);
                        relicDescription.text = GetRelics()[i].Description;
                    }
                    else
                    {
                        Debug.Log("선택한 유물의 이름과 갖고있는 유물의 이름이 다릅니다.");
                    }
                }
            }
        }
    }
}