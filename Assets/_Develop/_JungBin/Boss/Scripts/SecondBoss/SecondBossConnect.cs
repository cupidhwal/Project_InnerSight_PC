using JungBin;
using Seti;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SecondBossConnect : MonoBehaviour
{
    [SerializeField] private BossStageManager bossStageManager;
    [SerializeField] private GameObject Phase1;
    [SerializeField] private GameObject Phase2;
    [SerializeField] private Slider healthSlider; // 보스 체력 슬라이더
    [SerializeField] private float lerpSpeed = 2f; // 체력 감소 속도

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossStageManager.EnterBossStage(0);
    }

    public void PhaseChange()
    {
        Phase2.transform.position = Phase1.transform.position;
        Phase2.SetActive(true);
        BossStat bossStat = Phase2.GetComponent<BossStat>();
        bossStat.BossIsInvulnerable();
        
        Debug.Log("Phase2 보스 등장");
        Invoke("PhaseChangeVoid", 2f);
    }

    private void PhaseChangeVoid()
    {
        bossStageManager.EnterBossStage(1);
        FillHp();
    }

    private void FillHp()
    {
        StartCoroutine(LerpHPBar());
    }

    // 체력 값을 서서히 변경하는 코루틴
    private IEnumerator LerpHPBar()
    {
        float startHealth = 0;
        float endHealth = 100;

        float elapsedTime = 0f;
        while (elapsedTime < lerpSpeed)
        {
            elapsedTime += Time.deltaTime;
            healthSlider.value = Mathf.Lerp(startHealth, endHealth, elapsedTime / lerpSpeed);
            //Debug.Log(BossHealthBarSlider.value);
            yield return null;
        }

        // 최종값을 정확히 설정
        //BossHealthBarSlider.value = endHealth;
    }


}
