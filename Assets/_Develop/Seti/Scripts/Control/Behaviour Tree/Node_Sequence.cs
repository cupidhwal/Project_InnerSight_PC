using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Tree의 Sequence
    /// </summary>
    public class Node_Sequence : Node
    {
        // 필드
        private List<Node> children = new();

        // 오버라이드
        public override bool Execute()
        {
            foreach (var child in children)
            {
                if (!child.Execute()) return false; // 하나라도 실패하면 중단
            }
            return true; // 모든 행동이 성공하면 실행
        }

        // 메서드
        public void AddChild(Node node) => children.Add(node);
    }
}