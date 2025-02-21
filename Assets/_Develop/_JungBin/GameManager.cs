using Seti;
using System.Collections.Generic;
using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// 게임의 주요 시스템을 관리하는 클래스 (싱글톤)
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } // 싱글톤 인스턴스

        [SerializeField] private RelicManager relicManager;
        [SerializeField] private Player player;

        public Player Player => player; // 외부에서 접근 가능한 프로퍼티

        private void Awake()
        {
            // 싱글톤 인스턴스 설정
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        /// <summary>
        /// 유물을 등록하고 효과를 적용
        /// </summary>
        public void RegisterRelic(IRelic relic)
        {
            if (relic != null)
            {
                relicManager.AddRelic(relic, player);
            }
            else
            {
                Debug.LogWarning("유물 등록 실패: null 값이 전달됨.");
            }
        }
    }
}
