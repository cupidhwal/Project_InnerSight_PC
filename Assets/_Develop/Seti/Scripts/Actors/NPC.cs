using UnityEngine;

namespace Seti
{
    public class NPC : Actor
    {
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_NPC>();
    }
}