using UnityEngine;

namespace Seti
{
    public abstract class CompositionObject : ScriptableObject
    {
        public abstract void Execute(GameObject obj);
    }
}