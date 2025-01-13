using System.Collections.Generic;
using UnityEngine;

namespace JungBin
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } // 싱글톤 인스턴스

        [SerializeField] private RelicManager relicManager;
        [SerializeField] private Player player;
        [SerializeField] private Transform player_Transform;
        [SerializeField] private CapsuleCollider player_HitCapsuleCollider;
        [SerializeField] private BossStat bossStat;

        public Player Player => player; // 외부에서 접근 가능한 프로퍼티
        public Transform PlayerTransform => player_Transform;
        public CapsuleCollider Player_HitCapsuleCollider => player_HitCapsuleCollider;
        public BossStat BossStat => bossStat;

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