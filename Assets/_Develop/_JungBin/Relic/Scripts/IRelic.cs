using UnityEngine;

namespace JungBin
{

    public interface IRelic
    {
         string RelicName { get; }    // 유물 이름
         string Description { get; } // 유물 설명

        void ApplyEffect(Player player); // 유물 효과 적용
        void RemoveEffect(Player player); // 유물 효과 제거
    }
}