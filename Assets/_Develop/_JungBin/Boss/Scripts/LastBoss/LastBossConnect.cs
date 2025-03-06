using JungBin;
using UnityEngine;

public class LastBossConnet : MonoBehaviour
{
    [SerializeField] private BossStageManager bossStageManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossStageManager.EnterBossStage(0);
    }
}
