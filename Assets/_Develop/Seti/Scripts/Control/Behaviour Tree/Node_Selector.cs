using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Tree의 Selector
    /// </summary>
    public class Node_Selector : Node
    {
        // 필드
        private List<Node> children = new();

        // 오버라이드
        public override bool Execute()
        {
            foreach (var child in children)
            {
                if (child.Execute()) return true; // 하나라도 성공하면 실행 종료
            }
            return false; // 모든 행동이 실패하면 Idle 실행
        }

        // 메서드
        public void AddChild(Node node) => children.Add(node);
    }
}