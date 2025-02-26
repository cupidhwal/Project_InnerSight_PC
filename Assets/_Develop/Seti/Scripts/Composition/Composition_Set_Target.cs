using System.Collections;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// StoryManager의 Composition 리스트에 Target을 직접 할당할 수 없는 경우 필요한 Target을 세팅하는 SO (Root 필요)
    /// </summary>
    [CreateAssetMenu(fileName = "New Set Target Action", menuName = "Scenario/Composition/Object/Set Target")]
    public class Composition_Set_Target : CompositionObject
    {
        // 연출
        [Header("Variables")]
        [SerializeField]
        Transform targetRoot;

        public override void Execute(GameObject obj)
        {
            StoryManager.Instance.SetTempTarget(targetRoot.GetChild(0).gameObject);
        }
    }
}