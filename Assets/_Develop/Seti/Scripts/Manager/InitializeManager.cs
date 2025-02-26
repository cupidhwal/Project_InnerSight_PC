using UnityEngine;
using UnityEngine.Events;

namespace Seti
{
    /// <summary>
    /// 초기화 중심 클래스
    /// </summary>
    public class InitializeManager : Singleton<InitializeManager>
    {
        // 필드
        #region Variables
        public UnityAction Set_First;
        public UnityAction Set_Second;
        public UnityAction Set_Third;
        #endregion

        private void Start()
        {
            // 핵심
            Set_First?.Invoke();

            // 관리
            Set_Second?.Invoke();

            // 기타
            Set_Third?.Invoke();
        }
    }
}