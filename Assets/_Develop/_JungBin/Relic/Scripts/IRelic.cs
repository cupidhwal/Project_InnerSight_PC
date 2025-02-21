using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// 모든 유물이 구현해야 하는 인터페이스
    /// </summary>
    public interface IRelic
    {
        string RelicName { get; }  // 유물의 이름 (한글)
        string RelicID { get; }    // UI 버튼과 매칭할 영어 ID
        string Description { get; } // 유물 설명

        void ApplyEffect(); // 유물 효과 적용
        void RemoveEffect(); // 유물 효과 제거
    }
}
