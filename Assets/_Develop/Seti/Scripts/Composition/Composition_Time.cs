using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Time Action", menuName = "Scenario/Composition/Time/Scale")]
    public class Composition_Time : CompositionObject
    {
        [Header("Scale")]
        [SerializeField]
        private float timeScale;

        public override void Execute(GameObject _)
        {
            Time.timeScale = this.timeScale;
        }
    }
}