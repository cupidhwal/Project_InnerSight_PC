using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Switch Action", menuName = "Scenario/Composition/Object/Switch")]
    public class Composition_Switch : CompositionObject
    {
        public override void Execute(GameObject obj)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}