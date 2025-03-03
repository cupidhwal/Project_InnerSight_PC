using UnityEngine;
using System.Collections;

namespace Seti
{
    /// <summary>
    /// StoryManager의 Composition 리스트에 Target을 직접 할당할 수 없는 경우 필요한 Target을 세팅하는 SO (Root 필요)
    /// </summary>
    [CreateAssetMenu(fileName = "New Set NPC Action", menuName = "Scenario/Composition/Object/Set NPC")]
    public class Composition_Set_Target_NPC : CompositionObject
    {
        // 연출
        [Header("Variables")]
        [SerializeField]
        Transform targetRoot;
        [SerializeField]
        private int targetNPCIndex;

        public override void Execute(GameObject obj)
        {
            GameObject target = targetRoot.GetChild(4).GetChild(targetNPCIndex).gameObject;
            StoryManager.Instance.CurrentComp.target = target;
        }
    }
}