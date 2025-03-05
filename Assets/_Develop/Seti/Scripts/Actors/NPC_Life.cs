using UnityEngine;
using Noah;

namespace Seti
{
    /// <summary>
    /// 시나리오 진행 상태에 따른 상태 전환
    /// </summary>
    public class NPC_Life : MonoBehaviour
    {
        // 필드
        #region Variables
        [SerializeField]
        private int deathCount;

        private GameObject formAlive;
        private GameObject formDead;
        #endregion

        // 라이프 사이클
        private void Awake()
        {
            formAlive = transform.GetChild(0).gameObject;
            formDead = transform.GetChild(1).gameObject;
        }

        private void OnEnable()
        {
            StageManager.Instance.stageStartEvent += LifeChange;
        }

        private void OnDisable()
        {
            StageManager.Instance.stageStartEvent -= LifeChange;
        }

        // 메서드
        void LifeChange()
        {
            if (SaveLoadManager.Instance.scenarioSaveData.deathCount >= deathCount)
            {
                formAlive.SetActive(false);
                formDead.SetActive(true);
            }
            else
            {
                formAlive.SetActive(true);
                formDead.SetActive(false);
            }
        }
    }
}