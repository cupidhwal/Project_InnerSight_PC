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

        [SerializeField]
        private GameObject award;

        [Header("Variables: Exclusive")]
        [SerializeField]
        private GameObject enemyPrefab;
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
            if (enemyPrefab)
            {
                GameObject tutorialEnemy = Instantiate(enemyPrefab,
                                                       enemySummonPoint.position,
                                                       Quaternion.Euler(new Vector3(0f, 180f, 0f)),
                                                       Noah.StageManager.Instance.transform.GetChild(0).GetChild(1));
                Noah.StageManager.Instance.AddEnemy(tutorialEnemy);

                if (tutorialEnemy.TryGetComponent<Damagable>(out var damagable))
                {
                    damagable.OnDeath += ClearTutorial;
                }
            }

            trigger.enabled = false;

            StoryManager.Instance.OpenDialogue(2);
        }

        // Tutotial 끝 / 대화 시작
        private void ClearTutorial()
        {
            award.SetActive(true);
            Destroy(gameObject, 1);
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