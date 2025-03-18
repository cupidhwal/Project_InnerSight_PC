using Seti;
using System;
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
        [SerializeField] private Material[] bossMaterials; // 여러 개의 머티리얼 관리
        private Color[] originalColors; // 머티리얼 초기 색상 저장

        public Player Player => player; // 외부에서 접근 가능한 프로퍼티

        public static event Action OnStageChanged; // 🔹 스테이지 변경 이벤트
        public static event Action OnAnyStageChanged; // 🔹 스테이지 변경 이벤트

        private void Awake()
        {
            // 싱글톤 인스턴스 설정
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            originalColors = new Color[bossMaterials.Length];
            for (int i = 0; i < bossMaterials.Length; i++)
            {
                originalColors[i] = bossMaterials[i].color;
            }
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

        /// <summary>
        /// 스테이지 변경 시 호출됨
        /// </summary>
        public void ChangeStage()
        {
            Debug.Log("🔹 새로운 스테이지로 이동!");
            OnStageChanged?.Invoke(); // 🔹 이벤트 호출 → 유물 효과 적용됨!
            MaterialReset();
        }

        public void AnyChangeStage()
        {
            OnAnyStageChanged?.Invoke(); // 🔹 이벤트 호출 → 유물 효과 적용됨!
        }

        private void MaterialReset()
        {
            for (int i = 0; i < bossMaterials.Length; i++)
            {
                Color newColor = originalColors[i];
                newColor.a = 1f;
                bossMaterials[i].color = newColor;
            }
        }
    }
    
}
