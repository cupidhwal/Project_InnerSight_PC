using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 참조 관리
    /// </summary>
    public class Manager_Game : Singleton<Manager_Game>
    {
        // 속성
        public Player Player { get; private set; }
        public DialogUI DialogUI { get; private set; }

        // 라이프 사이클
        protected override void Awake()
        {
            base.Awake();

            // 참조
            Player = FindAnyObjectByType<Player>();
            DialogUI = GetComponentInChildren<DialogUI>();
        }
    }
}