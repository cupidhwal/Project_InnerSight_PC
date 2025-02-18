using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    public class Flynne : Actor
    {
        // 필드
        #region Variables
        [Header("Variables: Exclusive")]
        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private List<GameObject> enemies_Summoned = new();
        [SerializeField]
        private Vector3 enemySummonPoint;
        #endregion

        // 오버라이드
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_NPC>();

        // 라이프 사이클
        protected override void Start()
        {
            base.Start();
            enemySummonPoint = transform.Find("Root_Summon").position;
        }

        // 메서드
        // Enemy 소환
        private void GenEnemy()
        {
            Collider trigger = GetComponent<BoxCollider>();

            if (enemyPrefab)
            {
                GameObject tutorialEnemy = Instantiate(enemyPrefab,
                                                       enemySummonPoint,
                                                       Quaternion.Euler(new Vector3(0f, 180f, 0f)),
                                                       Noah.StageManager.Instance.transform.GetChild(0).GetChild(1));
                Noah.StageManager.Instance.AddEnemy(tutorialEnemy);

                if (tutorialEnemy.TryGetComponent<Damagable>(out var damagable))
                {
                    damagable.OnDeath += ClearTutorial;
                }
            }

            trigger.enabled = false;
        }

        // Tutotial 끝 / 대화 시작
        private void ClearTutorial()
        {

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