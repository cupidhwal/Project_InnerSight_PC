using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Character Action", menuName = "Scenario/Composition/Character/Life Change")]
    public class Composition_Character_Life : CompositionObject
    {
        private enum Life
        {
            Alive,
            Dead
        }

        // 필드
        [SerializeField]
        private Life lifeState;
        [SerializeField]
        private Transform targetCharacter;

        public override void Execute(GameObject _)
        {
            throw new System.NotImplementedException();
        }
    }
}