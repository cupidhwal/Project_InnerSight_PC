using System.Collections.Generic;
using UnityEngine;

namespace JungBin
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; // 싱글톤 패턴

        [SerializeField] private RelicManager relicManager;
        [SerializeField] private Player player;

        private void Awake()
        {
            // 싱글톤 인스턴스 설정
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        // 유물을 등록하고 효과를 적용
        public void RegisterRelic(IRelic relic)
        {
            if (relic != null)
            {
                relicManager.AddRelic(relic, player);
                
                Debug.Log($"유물 등록 유물의 이름 : {relic.RelicName}");
            }
        }
    }
}