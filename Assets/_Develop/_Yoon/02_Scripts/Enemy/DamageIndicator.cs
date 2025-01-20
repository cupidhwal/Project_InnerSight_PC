using UnityEngine;
using TMPro;

namespace Yoon
{

public class DamageIndicator : MonoBehaviour
{
    public TextMeshProUGUI damageText; // 데미지 표시 Text
    public float fadeSpeed = 2f;       // 텍스트 사라지는 속도
    public float moveSpeed = 1f;       // 텍스트 이동 속도

    private Color textColor;

    private void Start()
    {
        textColor = damageText.color; // 초기 색상 저장
    }

    private void Update()
    {
        // 텍스트를 위로 이동
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 텍스트 투명도 조정
        textColor.a -= fadeSpeed * Time.deltaTime;
        damageText.color = textColor;

        // 텍스트가 완전히 투명해지면 삭제
        if (textColor.a <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        damageText.text = damage.ToString(); // 데미지 값을 표시
    }
}

}