using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        void SetCurrentStage(int _stage)
        {
            curStage = _stage;

            if (currentStagePar.GetChild(0) != null)
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
        }

        public void NextStage()
        {
            player.GetComponent<Move>().enabled = false;

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

            player.GetComponent<Move>().enabled = true;

            player.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}