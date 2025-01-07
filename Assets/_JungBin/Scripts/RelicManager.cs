using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JungBin
{

    public class RelicManager : MonoBehaviour
    {
        private List<IRelic> relics = new List<IRelic>();
        [SerializeField] private TextMeshProUGUI relicDescription;      //���� ������ ���� �ؽ�Ʈ ������Ʈ

        // ���� �߰�
        public void AddRelic(IRelic relic, Player player)
        {
            relics.Add(relic);
            relic.ApplyEffect(player);
            Debug.Log($"{relic.RelicName} ������ ȿ�� : {relic.Description}");
        }

        // ���� ���� ��� ��ȯ
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
                        Debug.Log("������ ������ �̸��� �����ִ� ������ �̸��� �ٸ��ϴ�.");
                    }
                }
            }
        }
    }
}