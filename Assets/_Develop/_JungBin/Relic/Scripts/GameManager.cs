using System.Collections.Generic;
using UnityEngine;

namespace JungBin
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; // �̱��� ����

        [SerializeField] private RelicManager relicManager;
        [SerializeField] private Player player;

        private void Awake()
        {
            // �̱��� �ν��Ͻ� ����
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        // ������ ����ϰ� ȿ���� ����
        public void RegisterRelic(IRelic relic)
        {
            if (relic != null)
            {
                relicManager.AddRelic(relic, player);
                
                Debug.Log($"���� ��� ������ �̸� : {relic.RelicName}");
            }
        }
    }
}