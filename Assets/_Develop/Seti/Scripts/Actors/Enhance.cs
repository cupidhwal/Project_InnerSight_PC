using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Player 전용, 행동을 강화하는 기능을 관리하는 클래스
    /// </summary>
    /// increment는 백분율로 입력할 것
    /// Player 전용 클래스이므로 Player 컴포넌트를 강제한다
    [RequireComponent(typeof(Player))]

    [System.Serializable]
    public class Enhance : MonoBehaviour
    {
        // 필드
        #region Variables
        private Player player;
        private Dictionary<Type, IBehaviour> behaviourMap;

// 경고 무시
#pragma warning disable 0414
        [Header("Behaviour : Increments (%)")]
        [SerializeField] private float increment_Move = 10f;
#pragma warning restore 0414
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 초기화
            player = GetComponent<Player>();
            InitializeBehaviourMap();
        }
        #endregion

        // 메서드
        #region Methods
        // 행동 강화
        public void EnhanceBehaviour<T>(float increment) where T : class, IBehaviour
        {
            if (player == null) return;

            // 행동 검색 및 강화
            if (behaviourMap.TryGetValue(typeof(T), out var behaviour))
                (behaviour as T)?.Upgrade(increment);
        }

        // 행동 강화 - 오버로드, 중앙 집중식
        public void EnhanceBehaviour<T>() where T : class, IBehaviour
        {
            if (player == null) return;

            // 행동 검색
            if (!behaviourMap.TryGetValue(typeof(T), out var behaviour))
                return;

            // increment_ 필드 검색
            string incrementFieldName = $"increment_{typeof(T).Name}";
            FieldInfo incrementField = GetType().GetField(incrementFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (incrementField == null || incrementField.FieldType != typeof(float))
            {
                Debug.LogWarning($"'{incrementFieldName}'는 유효하지 않은 Increment 필드입니다.");
                return;
            }

            // 증가폭 가져오기
            float increment = (float)incrementField.GetValue(this);

            // 행동 강화
            (behaviour as T)?.Upgrade(increment);
        }

        // 행동 매핑
        private void InitializeBehaviourMap()
        {
            behaviourMap = new Dictionary<Type, IBehaviour>();

            foreach (var behaviour in player.Behaviours)
            {
                var type = behaviour.behaviour.GetType();
                behaviourMap[type] = behaviour.behaviour;
            }
        }
        #endregion
    }
}