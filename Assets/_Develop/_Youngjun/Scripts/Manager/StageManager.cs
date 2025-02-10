using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seti;
using Unity.AI.Navigation;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

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

        private GameObject nextStageObject;
        private GameObject randomSkillObject;

        private Transform enemyPar;
        private List<GameObject> enemys = new List<GameObject>();

        public int testStageChange;

        public GameObject CurrentStage => currentStage;

        // Test
        [SerializeField] private GameObject playerPrefab;
        InGameUI_Skill inGameUI_Skill;
        MinimapControl minimapControl;
        CinemachineCamera cinemachineCamera;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Init();
        }

        void Init()
        {
            currentStagePar = transform;
            player = FindAnyObjectByType<RayManager>().transform;

            StartCoroutine(ResetStage());

            inGameUI_Skill = FindAnyObjectByType<InGameUI_Skill>();
            minimapControl = FindAnyObjectByType<MinimapControl>();
        }

        IEnumerator ResetStage()
        {
            SetCurrentStage(testStageChange);

            yield return new WaitForSeconds(0.1f);

            GetCurrentStage();

            TestStageChage();
        }

        void SetCurrentStage(int _stage = 0)
        {
            curStage = _stage;

            if (currentStagePar.GetChild(0).gameObject != null)
            {
                Destroy(currentStagePar.GetChild(0).gameObject);
            }

            Instantiate(stageObject[curStage], currentStagePar);

            // 테스트용
            player.GetComponent<Condition_Player>().PlayerSetActive(false);
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<PlayerUseSkill>().enabled = false;
            player.GetComponent<Rigidbody>().useGravity = false;
        }

        void GetCurrentStage()
        {
            currentStage = currentStagePar.GetChild(0).gameObject;

            spawnPoint = currentStage.transform.Find("SpawnPoint");

            nextStageObject = currentStage.transform.GetChild(0).GetChild(0).gameObject;
            randomSkillObject = currentStage.transform.GetChild(0).GetChild(1).gameObject;

            enemyPar = currentStage.transform.GetChild(1);

            for (int i = 0; i < enemyPar.childCount; i++)
            {
                enemys.Add(enemyPar.GetChild(i).gameObject);
            }

            if (currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>() != null)
            {
                currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>().enabled = false;
            }


        }

        public void NextStage()
        {
            player.GetComponent<Condition_Player>().PlayerSetActive(false);
            player.GetComponent<PlayerUseSkill>().enabled = false;
            enemys.Clear();

            StartCoroutine(GoNextStage());
        }

        // Test
        public void NewStage()
        {
            player.GetComponent<Condition_Player>().PlayerSetActive(false);
            player.GetComponent<PlayerUseSkill>().enabled = false;
            enemys.Clear();

            StartCoroutine(ReStart());

            inGameUI_Skill.ResetSkill();
            minimapControl.SetPlayer();
        }

        public void ReStartGame()
        {
            NewStage();

            //SceneFade.instance.FadeOut("PlayScene");
        }

        // 테스트용
        void TestStageChage()
        {
            player.GetComponent<Condition_Player>().PlayerSetActive(true);
            player.GetComponent<NavMeshAgent>().enabled = true;
            player.GetComponent<PlayerUseSkill>().enabled = true;
            player.GetComponent<Rigidbody>().useGravity = true;

            if (currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>() != null)
            {
                currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>().enabled = true;
            }

        }

        // Test
        IEnumerator ReStart()
        {
            SceneFade.instance.FadeOut(null);

            yield return new WaitForSeconds(1f);

            player.GetComponent<Rigidbody>().useGravity = false;

            curStage = 0;

            yield return new WaitForSeconds(0.5f);

            Destroy(currentStage);

            Instantiate(stageObject[curStage], currentStagePar);

            yield return new WaitForSeconds(0.5f);

            GetCurrentStage();

            yield return new WaitForSeconds(0.5f);


            player.GetComponent<Condition_Player>().PlayerSetActive(true);
            player.GetComponent<PlayerUseSkill>().enabled = true;


            if (currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>() != null)
            {
                currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>().enabled = true;
            }

            player.GetComponent<NavMeshAgent>().enabled = false;

            //Instantiate(playerPrefab);

            //player = FindAnyObjectByType<RayManager>().transform;
            //cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();

            //cinemachineCamera.Follow = player.transform;
            //player.transform.position = spawnPoint.position;

            player.GetComponent<Rigidbody>().useGravity = true;

            yield return new WaitForSeconds(0.5f);

            player.GetComponent<NavMeshAgent>().enabled = true;

            SceneFade.instance.FadeIn(null);

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

            yield return new WaitForSeconds(0.5f);



            Destroy(currentStage);

            Instantiate(stageObject[curStage], currentStagePar);

            yield return new WaitForSeconds(0.5f);

            GetCurrentStage();

            yield return new WaitForSeconds(0.5f);


            player.GetComponent<Condition_Player>().PlayerSetActive(true);
            player.GetComponent<PlayerUseSkill>().enabled = true;


            if (currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>() != null)
            {
                currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>().enabled = true;
            }

            player.GetComponent<NavMeshAgent>().enabled = false;

            player.transform.position = spawnPoint.position;

            player.GetComponent<Rigidbody>().useGravity = true;

            yield return new WaitForSeconds(0.5f);

            player.GetComponent<NavMeshAgent>().enabled = true;

            SceneFade.instance.FadeIn(null);

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