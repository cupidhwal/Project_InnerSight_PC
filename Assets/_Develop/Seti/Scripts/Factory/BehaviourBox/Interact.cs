using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seti
{
    /// <summary>
    /// Move Behaviour
    /// </summary>
    [System.Serializable]
    public class Interact : IBehaviour
    {
        // 필드
        #region Variables
        // 전략 관리
        private Player player;
        #endregion

        // 인터페이스
        #region Interface
        // 초기화
        public void Initialize(Actor actor)
        {
            if (actor is Player)
                player = actor as Player;
        }

        public Type GetBehaviourType() => typeof(Interact);
        #endregion

        // 컨트롤러
        #region Controllers
        public void OnInteractStarted(InputAction.CallbackContext _) => OnInteraction();
        #endregion

        // 메서드
        #region Methods
        void OnInteraction()
        {
            if (player.CurrentNPC != null)
                player.CurrentNPC.Switch_TradeUI();
        }
        #endregion
    }
}