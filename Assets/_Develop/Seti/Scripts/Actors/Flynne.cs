using UnityEngine;

namespace Seti
{
    public class Flynne : Actor
    {
        // 오버라이드
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_NPC>();
    }
}