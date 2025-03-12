using UnityEngine;

namespace JungBin
{
    public class SecretKeyRelic : ResurrectionRelic
    {
        [SerializeField] private string relicName = "비밀 열쇠";
        [SerializeField] private string relicID = "Secret Key";
        [TextArea(5, 5)]
        [SerializeField] private string relicDescription = "다음 스테이지에 있는 히든 스테이지를 미리 파악 할 수 있습니다.";

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
                () => Debug.Log("히든 스테이지 탐지 가능"),
                () => Debug.Log("히든 스테이지 탐지 불가")
            );
        }

        public override void ApplyEffect()
        {
            //RelicEffectManager.ApplyEffect(RelicID);
        }

        public override void RemoveEffect()
        {
           // RelicEffectManager.RemoveEffect(RelicID);
        }
    }
}