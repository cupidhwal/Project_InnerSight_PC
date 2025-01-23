using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seti;
using JungBin;

namespace Noah
{
    public class StageManager : Singleton<StageManager>
    {
        private Transform player;
        private Transform currentStagePar;
        private GameObject currentStage;
        private Transform spawnPoint;
        public List<GameObject> stageObject = new List<GameObject>();

        private int curStage = 0;
        private bool isChangeScene = false;

        public GameObject nextStageObject;
        public GameObject randomSkillObject;

        public Transform enemyPar;
        public List<GameObject> enemys = new List<GameObject>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Init()
        {
            currentStagePar = transform;
            player = FindAnyObjectByType<RayManager>().transform;

            StartCoroutine(ResetStage());
        }

        IEnumerator ResetStage()
        {
            SetCurrentStage(5);

            yield return new WaitForSeconds(0.1f);

            GetCurrentStage();
        }

        void SetCurrentStage(int _stage = 0)
        {
            curStage = _stage;

            if (currentStagePar.GetChild(0).gameObject != null)
            {
                Destroy(currentStagePar.GetChild(0).gameObject);
            }

            Instantiate(stageObject[curStage], currentStagePar);
        }

        void GetCurrentStage()
        {
            currentStage = currentStagePar.GetChild(0).gameObject;

            spawnPoint = currentStage.transform.Find("SpawnPoint");

            player.transform.position = spawnPoint.position;

            nextStageObject = currentStage.transform.GetChild(0).GetChild(0).gameObject;
            randomSkillObject = currentStage.transform.GetChild(0).GetChild(1).gameObject;

            enemyPar = currentStage.transform.GetChild(1);

            for (int i = 0; i < enemyPar.childCount; i++)
            {
                enemys.Add(enemyPar.GetChild(i).gameObject);
            }
        }

        public void NextStage()
        {
            player.GetComponent<Condition_Player>().PlayerSetActive(false);
            player.GetComponent<PlayerUseSkill>().enabled = false;
            enemys.Clear();

            StartCoroutine(GoNextStage());
        }

        IEnumerator GoNextStage()
        {
            SceneFade.instance.FadeOut(null);

            yield return new WaitForSeconds(1f);

            player.GetComponent<Rigidbody>().useGravity = false;

            if (stageObject.Count == curStage + 1)
            {
                curStage = 0;
            }
            else
            {
                curStage += 1;
            }

            Destroy(currentStage);

            Instantiate(stageObject[curStage], currentStagePar);

            yield return new WaitForSeconds(0.5f);

            GetCurrentStage();

            yield return new WaitForSeconds(1f);

            SceneFade.instance.FadeIn(null);

            player.GetComponent<Condition_Player>().PlayerSetActive(true);
            player.GetComponent<PlayerUseSkill>().enabled = true;

            player.GetComponent<Rigidbody>().useGravity = true;
        }

        public void EnemyCount(GameObject _enemy)
        {
            enemys.Remove(_enemy);

            if(enemys.Count <= 0)
            {
                nextStageObject.SetActive(true);
                randomSkillObject.SetActive(true);
            }
        }
    }
}