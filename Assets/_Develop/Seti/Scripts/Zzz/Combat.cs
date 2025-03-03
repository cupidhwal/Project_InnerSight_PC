using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 이벤트성 전투
    /// </summary>
    public class Combat : MonoBehaviour
    {
        // 필드
        #region Variables
        private Collider trigger;

        [Header("Variables: Exclusive")]
        [SerializeField]
        private GameObject enemy;
        private GameObject tutorialEnemy;
        [SerializeField]
        private Transform enemySummonPoint;
        #endregion

        // 라이프 사이클
        private void Start()
        {
            trigger = GetComponent<BoxCollider>();
            enemySummonPoint = transform.GetChild(0);
        }

        // 메서드
        // Enemy 소환
        private void GenEnemy()
        {
            if (enemy)
            {
                tutorialEnemy = Instantiate(enemy,
                                            enemySummonPoint.position,
                                            Quaternion.Euler(new Vector3(0f, 180f, 0f)),
                                            Noah.StageManager.Instance.transform.GetChild(0).GetChild(1));
                Noah.StageManager.Instance.AddEnemy(tutorialEnemy);
            }

            trigger.enabled = false;

            StoryManager.Instance.SetTarget(tutorialEnemy);
            StoryManager.Instance.OpenDialogue(2);
        }

        // 이벤트 메서드
        #region Event Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GenEnemy();
            }
        }
        #endregion
    }
}