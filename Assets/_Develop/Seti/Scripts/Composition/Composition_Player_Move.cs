using UnityEngine;
using UnityEngine.AI;
using Noah;

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
            Player player = InitializeManager.Instance.Player;
            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();

            Vector3 targetPos = StageManager.Instance.CurrentStage.transform.GetChild(4).GetChild(2).position + new Vector3(0f, 0f, -2f);

            agent.enabled = false;
            player.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
            agent.enabled = true;
        }
    }
}