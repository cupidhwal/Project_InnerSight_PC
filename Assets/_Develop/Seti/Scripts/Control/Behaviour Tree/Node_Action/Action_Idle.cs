using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour Tree의 Action - 기본 행동
    /// </summary>
    public class Action_Idle : Node
    {
        public override bool Execute()
        {
            Debug.Log("AI: 대기 상태 유지...");
            return true;
        }
    }
}