using Noah;
using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Character Action", menuName = "Scenario/Composition/Character/Move")]
    public class Composition_Player_Move : CompositionObject
    {
        // 필드
        //[SerializeField]
        //private Vector3 targetPos;
        //[SerializeField]
        //private Vector3 targetRot;

        public override void Execute(GameObject _)
        {
            Vector3 targetPos = StageManager.Instance.CurrentStage.transform.GetChild(4).GetChild(2).position + new Vector3(0f, 0f, -2f);

            InitializeManager.Instance.Player.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
        }
    }
}