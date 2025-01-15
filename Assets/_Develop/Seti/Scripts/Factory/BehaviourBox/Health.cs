using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Health Behaviour - 오직 강화용, 전략이 존재할 수 없다
    /// </summary>
    public class Health : IBehaviour
    {
        // 필드
        #region Variables
        // 공격력
        private float health;
        private float health_Default;

        // 전략 관리
        private Actor actor;
        #endregion

        // 인터페이스
        #region Interface
        // 업그레이드
        public void Upgrade(float increment)
        {
            health += increment * health_Default / 100;
            actor.Update_Health(health);
            Initialize(actor);
        }

        // 초기화
        public void Initialize(Actor actor)
        {

            this.actor = actor;
            health_Default = actor.Health;
            
            if (actor is not Player)
            {
                health = health_Default;
                return;
            }

            // 이건 나중에 저장한 파일로부터 Load 하도록 바꿔야 함
            health = actor.Health;
        }

        public Type GetBehaviourType() => typeof(Defend);
        #endregion
    }
}