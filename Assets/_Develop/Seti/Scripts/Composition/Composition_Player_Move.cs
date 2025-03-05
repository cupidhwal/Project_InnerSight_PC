using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Character Action", menuName = "Scenario/Composition/Character/Move")]
    public class Composition_Player_Move : CompositionObject
    {
        // 필드
        [SerializeField]
        private Vector3 targetPos;

        public override void Execute(GameObject _)
        {
            InitializeManager.Instance.Player.transform.position = targetPos;
        }
    }
}