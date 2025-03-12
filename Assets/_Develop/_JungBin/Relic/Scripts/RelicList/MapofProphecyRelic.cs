using UnityEngine;

namespace JungBin
{
    public class MapofProphecyRelic : ResurrectionRelic
    {
        [SerializeField] private string relicName = "마경의 예지도";
        [SerializeField] private string relicID = "Map of Prophecy";
        [TextArea(5, 5)]
        [SerializeField] private string relicDescription = "다음 스테이지에서 등장할 몬스터의 숫자를 미리 확인할 수 있습니다.";

        // private Player player;

        public override string RelicName => relicName;
        public override string RelicID => relicID;
        public override string Description => relicDescription;

        /// <summary>
        /// 🔹 유물 효과를 등록하는 `Awake()` (테스트 유물 전용 설정 추가)
        /// </summary>
        protected override void Awake()
        {
            // 🔹 새로운 유물만의 특별한 효과 등록 가능!
            RelicEffectManager.RegisterEffect(RelicID,
                () => Debug.Log("다음 스테이지의 몬스터의 숫자 표시"),
                () => Debug.Log("다음 스테이지의 몬스터의 숫자 표시 제거")
            );
        }

        public override void ApplyEffect()
        {
            //RelicEffectManager.ApplyEffect(RelicID);
        }

        public override void RemoveEffect()
        {
            //RelicEffectManager.RemoveEffect(RelicID);
        }
    }
}