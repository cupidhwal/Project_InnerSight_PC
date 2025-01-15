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
        // 공격력
        [SerializeField] private float defend;

        // 전략 관리
        private Actor actor;
        #endregion

        // 인터페이스
        #region Interface
        // 업그레이드
        public void Upgrade(float increment)
        {
            defend += increment * actor.Defend_Default / 100;
            actor.Update_Defend(defend);
        }

        // 초기화
        public void Initialize(Actor actor)
        {
            if (actor is not Player)
            {
                Debug.Log("Defend Behaviour는 Player만 사용할 수 있습니다.");
                return;
            }

            this.actor = actor;
            defend = actor.Defend;  // 이건 나중에 저장한 파일로부터 Load 하도록 바꿔야 함
        }

        public Type GetBehaviourType() => typeof(Defend);
        #endregion
    }
}