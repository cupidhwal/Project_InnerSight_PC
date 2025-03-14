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
                () => GameManager.OnStageChanged += HealPlayer,
                () => GameManager.OnStageChanged -= HealPlayer
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

        /// <summary>
        /// 스테이지 이동 시 체력 회복 (기존 함수 활용)
        /// </summary>
        private void HealPlayer()
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
            float healAmount = damagable.MaxHitPoint / 20;
            if (damagable.MaxHitPoint == damagable.CurrentHitPoints)
            {
                Debug.Log("생명력 회복 없음");
                return;
            }
            else if(damagable.MaxHitPoint - damagable.CurrentHitPoints < healAmount)
            {
                damagable.HealCurrentHitPoint(damagable.MaxHitPoint - damagable.CurrentHitPoints);
                Debug.Log("남은 체력 다 회복");
            }
            else if (damagable.MaxHitPoint - damagable.CurrentHitPoints >= healAmount)
            {
                damagable.HealCurrentHitPoint(healAmount); // ✅ 기존 체력 회복 함수 호출
                Debug.Log("5퍼센트 회복");
            }

            Debug.Log($"🔹 체력 강화 효과 적용됨! +{healAmount} HP 증가");
        }
    }
}