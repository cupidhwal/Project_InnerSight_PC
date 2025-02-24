using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

namespace Seti
{
    /// <summary>
    /// Environment 관리 클래스
    /// </summary>
    public class Environment : MonoBehaviour
    {
        // 필드
        public List<NavMeshModifier> Modifiers { get; private set; } = new();

        // 라이프 사이클
        private void Start()
        {
            Initialize();
        }

        // 메서드
        void Initialize() => Modifiers.AddRange(GetComponentsInChildren<NavMeshModifier>());
    }
}