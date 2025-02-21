using JungBin;
using UnityEngine;

/// <summary>
/// 새로운 유물 (테스트 유물)
/// </summary>
public class Test : ResurrectionRelic
{
    [SerializeField] private string relicName = "테스트 유물";
    [SerializeField] private string relicID = "TestRelic";
    [TextArea(5, 5)]
    [SerializeField] private string relicDescription = "이 유물은 테스트를 위한 유물입니다.";

    public override string RelicName => relicName;
    public override string RelicID => relicID;
    public override string Description => relicDescription;

    /// <summary>
    /// 🔹 유물 효과를 등록하는 `Awake()` (테스트 유물 전용 설정 추가)
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); // 부모의 Awake() 실행 (기본 등록 유지)

        // 🔹 새로운 유물만의 특별한 효과 등록 가능!
        RelicEffectManager.RegisterEffect(RelicID,
            () => Debug.Log("테스트 유물 효과 적용!"),
            () => Debug.Log("테스트 유물 효과 제거!")
        );
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect(); // 기본 효과 유지
        Debug.Log("테스트 유물 효과 추가 실행!");
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect(); // 기본 효과 유지
        Debug.Log("테스트 유물 효과 제거 실행!");
    }
}
