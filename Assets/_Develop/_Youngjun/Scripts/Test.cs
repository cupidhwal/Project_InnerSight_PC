using JungBin;
using UnityEngine;

public class Test : ResurrectionRelic
{
    public override string RelicName => "테스트";       //유물의 이름
    public override string RelicID => "Test";  // UI 버튼과 매칭될 영어 ID
    public override string Description => "Test Description";       //유물 설명
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void ApplyEffect(Player player)
    {
        base.ApplyEffect(player);
    }

    public override void RemoveEffect(Player player)
    {
        base.RemoveEffect(player);
    }
}
