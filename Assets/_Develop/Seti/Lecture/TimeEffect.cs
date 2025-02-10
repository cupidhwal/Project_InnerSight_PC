using UnityEngine;
using System.Collections;

namespace Seti
{
    /// <summary>
    /// 플레이어 공격 이펙트 애니메이션 플레이
    /// </summary>
    public class TimeEffect : MonoBehaviour
    {
        // 필드
        #region Variables
        //public Light weaponLight;
        private Animation m_Animation;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Awake()
        {
            // 참조
            m_Animation = GetComponent<Animation>();

            // 초기화
            gameObject.SetActive(false);
        }
        #endregion

        // 메서드
        #region Methods
        // 이펙트 연출
        public void Activate()
        {
            gameObject.SetActive(true);
            //weaponLight.enabled = true;

            if (m_Animation != null)
            {
                m_Animation.Play();

                // 이펙트 초기화
                StartCoroutine(DisableAtEndOfAnimation());
            }
        }
        #endregion

        // 유틸리티
        #region Utilities
        IEnumerator DisableAtEndOfAnimation()
        {
            Player player = GetComponentInParent<Player>();
            float timeStamp = Time.time;
            while (timeStamp + m_Animation.clip.length > Time.time)
            {
                if (player.Condition.IsDash)
                    break;
                yield return null;
            }
            
            gameObject.SetActive(false);
            //weaponLight.enabled = false;

            yield break;
        }
        #endregion
    }
}