using Noah;
using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Event Update Action", menuName = "Scenario/Composition/Event/NPC A Event Update")]
    public class Composition_Update_NPC_A_Event : CompositionObject
    {
        public override void Execute(GameObject obj)
        {
            DataManager.Instance.dialogueDatas[0].CheckSeens[1] = true;
            SaveLoadManager.Instance.SaveScenario(DataManager.Instance.DialogueData);
        }
    }
}