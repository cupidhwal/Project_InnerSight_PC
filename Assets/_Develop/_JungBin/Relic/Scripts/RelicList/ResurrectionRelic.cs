using Noah;
using Seti;
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

        private string actionUI_Text = "유물 획득";
        private bool isContact = false;

        /// <summary>
        /// 🔹 유물 효과를 등록하는 `Awake()` (각 유물에서 개별 설정 가능)
        /// </summary>
        protected virtual void Awake()
        {
            // 🔹 GameManager.Instance.Player가 존재하는지 확인
            if (GameManager.Instance == null || GameManager.Instance.Player == null)
            {
                Debug.LogError("❌ GameManager.Instance.Player가 초기화되지 않았음.");
                return;
            }

            // 🔹 플레이어 객체 가져오기
            Condition_Player playerCondition = GameManager.Instance.Player.GetComponent<Condition_Player>();

            if (playerCondition == null)
            {
                Debug.LogError("❌ Condition_Player 컴포넌트를 찾을 수 없음.");
                return;
            }

            Debug.Log($"✅ Condition_Player 확인됨: {playerCondition}");

            // 🔹 유물 효과를 중앙 관리 시스템에 등록
            RelicEffectManager.RegisterEffect(RelicID,
                () => ApplyEffect(),  // ApplyEffect()
                () => RemoveEffect()  // RemoveEffect()
            );
        }

        private void Update()
        {
            if (isContact)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    foreach (var relic in SaveLoadManager.Instance.relicSaveData.relics)
                    {
                        if (relic.relicName == RelicName) return; // 중복 획득 방지
                    }

                    ActionUIManager.Instance.DisableActionUI();

                    isContact = false;

                    // 🔹 유물 등록 및 효과 적용
                    RelicManager.Instance.AddRelic(this, GameManager.Instance.Player);

                    // 🔹 유물 습득 후 오브젝트 삭제
                    Destroy(gameObject);
                }
            }
        }


        public virtual void ApplyEffect()
        {
            Condition_Player playerCondition = GameManager.Instance.Player.GetComponent<Condition_Player>();
            playerCondition.SetLife(1);
        }


        public virtual void RemoveEffect()
        {
            Condition_Player playerCondition = GameManager.Instance.Player.GetComponent<Condition_Player>();
            playerCondition.SetLife(-1);
        }

        /// <summary>
        /// 플레이어가 유물과 접촉하면 습득
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isContact = true;

                ActionUIManager.Instance.EnableActionUI(RelicName);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isContact = false;

                ActionUIManager.Instance.DisableActionUI();
            }
        }
    }
}
