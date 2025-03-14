using Seti;
using UnityEngine;

namespace JungBin
{
    public class PowerOfElixirRelic : ResurrectionRelic
    {
        [SerializeField] private string relicName = "힘의 영약";
        [SerializeField] private string relicID = "Power Of Elixir";
        [TextArea(5, 5)]
        [SerializeField] private string relicDescription = "스테이지 입장시 첫 공격의 위력이 증가합니다";


        [SerializeField] private float bonusAttack;
        //[SerializeField] private float healAmount;

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
                () => ApplyEffect(),
                () => RemoveEffect()
            );
        }

        public override void ApplyEffect()
        {
            //RelicEffectManager.ApplyEffect(RelicID);
            GameManager.OnAnyStageChanged += OneChanceAttack;
        }

        public override void RemoveEffect()
        {
            //RelicEffectManager.RemoveEffect(RelicID);
            GameManager.OnAnyStageChanged -= OneChanceAttack;
        }

        private void OneChanceAttack()
        {
            if (GameManager.Instance.Player == null)
            {
                Debug.Log("GameManager.Instance.Player == null");
                return;
            }

            Damagable damagable = GameManager.Instance.Player.GetComponent<Damagable>();
            if (damagable == null)
            {
                Debug.Log("damagable == null");
                return;
            }

            //bonusAttack == 이 값만 넣으면 됌 위에 Damageable은 예시
        }
    }
}