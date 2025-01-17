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
        [SerializeField] private float health;

        // 전략 관리
        private Actor actor;
        #endregion

        // 인터페이스
        #region Interface
        // 업그레이드
        public void Upgrade(float increment)
        {
            if (actor is not Player) return;

            health = actor.Health;
            health += increment * actor.Health_Default / 100;
            actor.Update_Health(health);
        }

        // 초기화
        public void Initialize(Actor actor)
        {
            if (actor is not Player)
            {
                Debug.Log("Health Behaviour는 Player만 사용할 수 있습니다.");
                return;
            }

            this.actor = actor;
        }

        public Type GetBehaviourType() => typeof(Health);
        #endregion
    }
}