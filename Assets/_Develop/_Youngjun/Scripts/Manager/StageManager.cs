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
            GetCurrentStage();
        }

        void GetCurrentStage()
        {
            currentStage = currentStagePar.GetChild(0).gameObject;
        }

        public void NextStage()
        {
            StartCoroutine(GoNextStage());
        }

        IEnumerator GoNextStage()
        {
            SceneFade.instance.FadeOut(null);

            yield return new WaitForSeconds(1f);

            player.GetComponent<Rigidbody>().useGravity = false;

            curStage += 1;

            Destroy(currentStage);

            Instantiate(stageObject[curStage], currentStagePar);    

            yield return new WaitForSeconds(0.5f);

            player.GetComponent<Rigidbody>().useGravity = true;

            GetCurrentStage();

            SceneFade.instance.FadeIn(null);
        }
    }
}