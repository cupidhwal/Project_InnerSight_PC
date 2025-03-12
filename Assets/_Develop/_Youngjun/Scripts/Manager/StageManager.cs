using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seti;
using Unity.AI.Navigation;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Diagnostics;
using InnerSight_Kys;
using Unity.VisualScripting;

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
        private GameObject skillReinObject;
        private GameObject statsReinObject;

        private Transform enemyPar;
        [SerializeField] private List<GameObject> enemys = new List<GameObject>();

        // 히든 스테이지
        public GameObject hiddenStage;
        private GameObject hiddenPotal;
        public Vector3 playerPos;

        private bool isHidden = false;

        public GameObject gameOverUI;

        public bool IsHidden
        {
            get { return isHidden; }
            set { isHidden = value; }
        }

        public UnityAction stageStartEvent;
        public UnityAction stageEndEvent;

        public GameObject CurrentStage => currentStage;
        public List<GameObject> Enemies => enemys;

        public List<float> reinforceData = new List<float>();

        HiddenEntryObject hiddenEntryObject;

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

            SetBGMSound();
        }

        IEnumerator ResetStage()
        {
            SetCurrentStage();

            yield return new WaitForSeconds(0.1f);

            GetCurrentStage();
        }

        void SetCurrentStage(int _stage = 0)
        {
            if (!SaveLoadManager.Instance.isTutorial)
            {
                curStage = _stage;
            }
            else
            {
                curStage = 1;
            }


            if (currentStagePar.GetChild(0).gameObject != null)
            {
                Destroy(currentStagePar.GetChild(0).gameObject);
            }

            Instantiate(stageObject[curStage], currentStagePar);

            player.GetComponent<Condition_Player>().PlayerSetActive(false);
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<PlayerUseSkill>().enabled = false;
            player.GetComponent<Rigidbody>().useGravity = false;
        }

        void GetCurrentStage()
        {
            stageStartEvent?.Invoke();

            if (!isHidden)
            {
                currentStage = currentStagePar.GetChild(0).gameObject;

                nextStageObject = currentStage.transform.GetChild(0).GetChild(0).gameObject;
                randomSkillObject = currentStage.transform.GetChild(0).GetChild(1).gameObject;

                if (currentStage.transform.GetChild(0).childCount >= 3)
                {
                    hiddenPotal = currentStage.transform.GetChild(0).GetChild(2).gameObject;
                }
            }
            else
            {
                currentStage = currentStagePar.GetChild(1).gameObject;

                nextStageObject = currentStage.transform.GetChild(0).GetChild(0).gameObject;
                skillReinObject = currentStage.transform.GetChild(0).GetChild(1).gameObject;
                statsReinObject = currentStage.transform.GetChild(0).GetChild(2).gameObject;

                HiddenStageManager.Instance.SetObject();
            }

            spawnPoint = currentStage.transform.Find("SpawnPoint");
            enemyPar = currentStage.transform.GetChild(1);

            for (int i = 0; i < enemyPar.childCount; i++)
            {
                enemys.Add(enemyPar.GetChild(i).gameObject);
            }

            if (currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>() != null)
            {
                currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>().enabled = false;
            }

            if (curStage == 0 || curStage == 1)
            {
                player.transform.position = spawnPoint.position;

                if (currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>() != null)
                {
                    currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>().enabled = true;
                }

                player.GetComponent<NavMeshAgent>().enabled = true;
                player.GetComponent<PlayerUseSkill>().enabled = true;
                player.GetComponent<Rigidbody>().useGravity = true;
            }
            stageEndEvent?.Invoke();
        }

        void EscapeHiddenStage()
        {
            currentStage = currentStagePar.GetChild(0).gameObject;

            if (currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>() != null)
            {
                currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>().enabled = false;
            }


        }

        public void NextStage()
        {
            //player.GetComponent<Condition_Player>().PlayerSetActive(false);

            stageStartEvent?.Invoke();

            player.GetComponent<PlayerUseSkill>().enabled = false;
            enemys.Clear();

            StartCoroutine(GoNextStage());
        }

        public void ReturnCurrentStage()
        {
            player.GetComponent<Condition_Player>().PlayerSetActive(false);
            player.GetComponent<PlayerUseSkill>().enabled = false;
            enemys.Clear();

            StartCoroutine(GoCurrentStage());
        }

        public void ReStartGame()
        {
            AudioManager.Instance.Play("Game Over");

            Invoke("SetActiveDelay", 1f);

            SceneFade.instance.FadeOut(SceneManager.GetActiveScene().name, 5f);
        }

        void SetActiveDelay()
        {
            gameOverUI.SetActive(true);
        }

        // 일반 스테이지 전환 및 히든 스테이지
        IEnumerator GoNextStage()
        {
            SceneFade.instance.FadeOut(null);

            yield return new WaitForSeconds(1f);

            player.GetComponent<Rigidbody>().useGravity = false;

            if (!isHidden)
            {
                if (curStage == 0)
                {
                    curStage = 2;
                }
                else
                {
                    if (stageObject.Count == curStage + 1)
                    {
                        curStage = 0;
                    }
                    else
                    {
                        curStage += 1;
                    }
                }
            
            }

            yield return new WaitForSeconds(0.5f);

            if (!isHidden)
            {
                Destroy(currentStage);

                Instantiate(stageObject[curStage], currentStagePar);

                SetBGMSound();
            }
            else
            {
                currentStage.SetActive(false);

                Instantiate(hiddenStage, currentStagePar);

                // 히든 스테이지 BGM 재생
                SetHiddenStageBGM();
            }

            yield return new WaitForSeconds(0.5f);

            GetCurrentStage();

            yield return new WaitForSeconds(0.5f);

            //player.GetComponent<Condition_Player>().PlayerSetActive(true);
            stageEndEvent?.Invoke();
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

        // 히든 던전 빠져나가는 코루틴
        IEnumerator GoCurrentStage()
        {
            SceneFade.instance.FadeOut(null);

            yield return new WaitForSeconds(1f);

            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Collider>().enabled = false;

            yield return new WaitForSeconds(0.5f);

            Destroy(currentStagePar.GetChild(1).gameObject);

            currentStagePar.GetChild(0).gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            EscapeHiddenStage();

            yield return new WaitForSeconds(0.5f);

            player.GetComponent<Condition_Player>().PlayerSetActive(true);
            player.GetComponent<PlayerUseSkill>().enabled = true;

            if (currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>() != null)
            {
                currentStage.transform.GetChild(2).GetComponent<NavMeshSurface>().enabled = true;
            }

            player.GetComponent<NavMeshAgent>().enabled = false;

            player.transform.position = playerPos;

            player.GetComponent<Collider>().enabled = true;
            player.GetComponent<Rigidbody>().useGravity = true;

            yield return new WaitForSeconds(0.5f);

            player.GetComponent<NavMeshAgent>().enabled = true;

            SceneFade.instance.FadeIn(null);

            SetBGMSound();
        }

        public void EnemyCount(GameObject _enemy)
        {
            enemys.Remove(_enemy);

            if(enemys.Count <= 0)
            {
                nextStageObject.SetActive(true);

                if (!isHidden)
                {
                    if (randomSkillObject != null)
                    {
                        randomSkillObject.SetActive(true);
                    }
                    if (hiddenPotal != null)
                    {
                        hiddenPotal.SetActive(true);
                    }
                    if (ComponentUtility.TryGetComponentInChildren<Trigger_Stage>(transform, out var stageTrigger))
                    {
                        stageTrigger.OpenDialogue();
                    }
                }
                else
                {
                    skillReinObject.SetActive(true);
                    statsReinObject.GetComponent<Collider>().enabled = true;
                    statsReinObject.transform.GetChild(0).gameObject.SetActive(true);
                }


                if (currentStage.transform.GetChild(0).childCount >= 3)
                {
                    hiddenEntryObject = currentStage.transform.GetChild(0).GetChild(2).GetComponent<HiddenEntryObject>();

                    if (hiddenEntryObject != null)
                    {
                        for (int i = 0; i < hiddenEntryObject.reinforceData.Count; i++)
                        {
                            reinforceData.Add(hiddenEntryObject.reinforceData[i]);
                        }
                    }


                }


            }
        }

        public void AddEnemy(GameObject enemy) => enemys.Add(enemy);

        public void SetBGMSound()
        {
            switch (curStage)
            {
                case 0:
                    AudioManager.Instance.PlayBgm("Tutorial,0");
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    AudioManager.Instance.PlayBgm("1~4");
                    break;
                case 5:
                    AudioManager.Instance.PlayBgm("5");
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    AudioManager.Instance.PlayBgm("6~9");
                    break;
                case 10:
                    AudioManager.Instance.PlayBgm("10");
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                    AudioManager.Instance.PlayBgm("11~14");
                    break;
                case 15:
                    AudioManager.Instance.PlayBgm("15");
                    break;
                default:
                    return;
            }
        }

        void SetHiddenStageBGM()
        {
            AudioManager.Instance.PlayBgm("Hidden Scene");
        }
    }
}