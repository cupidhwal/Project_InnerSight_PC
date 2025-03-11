using Seti;
using UnityEngine;

namespace JungBin
{
    public class HealingStoneRelic : ResurrectionRelic
    {
        [SerializeField] private string relicName = "회복의 돌";
        [SerializeField] private string relicID = "Healing Stone";
        [TextArea(5, 5)]
        [SerializeField] private string relicDescription = "다음 스테이지로 이동 시 일정 체력을 회복합니다.";

       // private Player player;

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
                () => Debug.Log("회복의 돌 유물 효과 추가!"),
                () => Debug.Log("회복의 돌 유물 효과 제거!")
            );
        }

        public override void ApplyEffect()
        {
            RelicEffectManager.ApplyEffect(RelicID);
        }

        public override void RemoveEffect()
        {
            RelicEffectManager.RemoveEffect(RelicID);
        }
    }
}