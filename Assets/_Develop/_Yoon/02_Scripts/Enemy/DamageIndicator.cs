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
            if (damageText != null)
            {
                textColor = damageText.color; // 초기 색상 저장
            }
            else
            {
                Debug.LogError("DamageText is not assigned!");
            }

            Destroy(gameObject, 1.5f); // 1.5초 후 자동 삭제
        }

        private void Update()
        {
            // 텍스트가 존재하는 경우에만 처리
            if (damageText != null)
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
        }

        public void SetDamage(float damage)
        {
            if (damageText != null)
            {
                damageText.text = damage.ToString(); // 데미지 숫자를 문자열로 변환하여 텍스트에 표시
            }
            else
            {
                Debug.LogError("DamageText is not assigned in the prefab!");
            }
        }

    }

}