using UnityEngine;
using Noah;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Set Boss Action", menuName = "Scenario/Composition/Object/Set Boss")]
    public class Composition_Set_Target_Boss : CompositionObject
    {
        public override void Execute(GameObject obj)
        {
            GameObject target = StageManager.Instance.CurrentStage.transform.GetChild(1).GetChild(0).gameObject;
            StoryManager.Instance.CurrentComp.target = target;
        }
    }
}