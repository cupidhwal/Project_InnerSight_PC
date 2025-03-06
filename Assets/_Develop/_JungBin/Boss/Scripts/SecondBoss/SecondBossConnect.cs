using JungBin;
using UnityEngine;

public class SecondBossConnect : MonoBehaviour
{
    [SerializeField] private BossStageManager bossStageManager;
    [SerializeField] private GameObject Phase1;
    [SerializeField] private GameObject Phase2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossStageManager.EnterBossStage(0);
    }

    public void PhaseChange()
    {
        Phase2.transform.position = Phase1.transform.position;
        Phase2.SetActive(true);
        Debug.Log("Phase2 보스 등장");
        Invoke("PhaseChangeVoid", 3.5f);
    }

    private void PhaseChangeVoid()
    {
        bossStageManager.EnterBossStage(1);
    }


}
