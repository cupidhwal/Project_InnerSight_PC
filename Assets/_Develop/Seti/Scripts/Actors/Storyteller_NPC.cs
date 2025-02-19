using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 단역
    /// </summary>
    public class Storyteller_NPC : Storyteller
    {
        // 필드
        [Header("Criteria : AI Behaviour")]
        [SerializeField]
        protected Player player;
        [SerializeField]
        protected float range_Event = 2f;
        [SerializeField]
        protected float distanceToPlayer = 0f;

        // 초기화
        protected override void Initialize()
        {
            base.Initialize();
            player = Manager_Game.Instance.Player;
        }

        // 이벤트
        public override void StoryEnter()
        {
            // 거리 계산
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (!dialogue.isRead && distanceToPlayer < range_Event)
            {
                OnStoryEnter?.Invoke();
            }
        }
    }
}