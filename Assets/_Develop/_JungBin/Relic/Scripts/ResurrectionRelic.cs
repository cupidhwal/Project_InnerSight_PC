using UnityEngine;

namespace JungBin
{
    /// <summary>
    /// 생명의 고리 유물 (죽을 때 1번 부활 가능)
    /// </summary>
    public class ResurrectionRelic : MonoBehaviour, IRelic
    {
        public virtual string RelicName => "생명의 고리";
        public virtual string RelicID => "ResurrectionRing";
        public virtual string Description => "죽음을 맞이하는 순간, 단 한 번 생명력을 되찾아 다시 일어섭니다.";

        /// <summary>
        /// 🔹 유물 효과를 등록하는 `Awake()` (각 유물에서 개별 설정 가능)
        /// </summary>
        protected virtual void Awake()
        {
                      // 🔹 유물 효과를 중앙 관리 시스템에 등록
            RelicEffectManager.RegisterEffect(RelicID,
                () => Player.SetLives(Player.Lives + 1),  // ApplyEffect()
                () => Player.SetLives(Player.Lives - 1)   // RemoveEffect()
            );
        }

        public virtual void ApplyEffect()
        {
            RelicEffectManager.ApplyEffect(RelicID);
        }

        public virtual void RemoveEffect()
        {
            RelicEffectManager.RemoveEffect(RelicID);
        }

        /// <summary>
        /// 플레이어가 유물과 접촉하면 습득
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                foreach (var relic in RelicManager.Instance.GetRelics())
                {
                    if (relic.RelicID == RelicID) return; // 중복 획득 방지
                }

                // 🔹 유물 등록 및 효과 적용
                RelicManager.Instance.AddRelic(this, other.gameObject.GetComponent<Player>());

                // 🔹 유물 습득 후 오브젝트 삭제
                Destroy(gameObject);
            }
        }
    }
}
