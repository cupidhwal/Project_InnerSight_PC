using System;

namespace Seti
{
    /// <summary>
    /// Stagger Behaviour
    /// </summary>
    /// 경직 행동
    public class Stagger : IBehaviour
    {
        // 필드
        #region Variables
        // 스탯
        private float stagger_Interval;

        // 전략 관리
        private Actor actor;
        #endregion

        // 인터페이스
        #region Interface
        // 초기화
        public void Initialize(Actor actor)
        {
            this.actor = actor;
            stagger_Interval = actor.Stagger;   // 이건 나중에 저장한 파일로부터 Load 하도록 바꿔야 함
        }

        public Type GetBehaviourType() => typeof(Stagger);
        #endregion
    }
}