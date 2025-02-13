using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Defend Behaviour
    /// </summary>
    public class Defend : IBehaviour
    {
        // 필드
        #region Variables
        // 방어력
        [SerializeField] private float defend;

        // 전략 관리
        private Actor actor;
        #endregion

        // 인터페이스
        #region Interface
        // 초기화
        public void Initialize(Actor actor)
        {
            if (actor is not Player)
            {
                Debug.Log("Defend Behaviour는 Player만 사용할 수 있습니다.");
                return;
            }

            this.actor = actor;
        }

        public Type GetBehaviourType() => typeof(Defend);
        #endregion
    }
}