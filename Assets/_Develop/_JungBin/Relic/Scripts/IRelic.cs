using UnityEngine;

namespace JungBin
{

    public interface IRelic
    {
        string RelicName { get; }  // 한글 이름
        string RelicID { get; }    // UI 버튼과 매칭할 영어 ID
        string Description { get; } // 설명

        void ApplyEffect(Player player);
        void RemoveEffect(Player player);
    }

}