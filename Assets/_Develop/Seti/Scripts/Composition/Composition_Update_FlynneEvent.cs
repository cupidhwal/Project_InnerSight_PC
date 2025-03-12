using UnityEngine;
using Noah;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Event Update Action", menuName = "Scenario/Composition/Event/Flynne Event Update")]
    public class Composition_Update_FlynneEvent : CompositionObject
    {
        [Header("Variables")]
        [SerializeField]
        private int eventIndex;

        public override void Execute(GameObject obj)
        {
            DataManager.Instance.flynneEvent[eventIndex] = true;
            SaveLoadManager.Instance.SaveScenario(DataManager.Instance.DialogueData);
        }
    }
}