using UnityEngine;

public class DashController : MonoBehaviour
{
    [SerializeField] private float minScale = 0.5f; // 최소 스케일
    [SerializeField] private float maxScale = 0.7f; // 최대 스케일
    [SerializeField] private float scaleSpeed = 2f; // 스케일 증가 속도

    private float currentScale;
    private bool isGrowing = true; // 현재 증가 중인지 확인

    void Start()
    {
        currentScale = minScale;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, currentScale);
    }

    void Update()
    {
        if (isGrowing)
        {
            currentScale += scaleSpeed * Time.deltaTime;
            if (currentScale >= maxScale)
            {
                currentScale = minScale; // 4가 되면 즉시 1로 초기화
                isGrowing = true; // 다시 증가 시작
            }
        }

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, currentScale);
    }
}
