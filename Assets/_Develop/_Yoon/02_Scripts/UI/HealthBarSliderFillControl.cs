using UnityEngine;
using UnityEngine.UI;

namespace Yoon
{

    public class HealthBarSliderFillControl : MonoBehaviour
    {
        //
        public Slider slider; // 슬라이더
        public Image fillImage; // Fill 이미지

        void Update()
        {
            if (slider.value <= 0)
            {
                fillImage.enabled = false;  // 값이 0이면 이미지 비활성화
            }
            else
            {
                fillImage.enabled = true;   // 값이 0보다 크면 이미지 활성화
            }
        }
    }

}