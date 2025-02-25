using System.Collections;
using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Instantiate Action", menuName = "Scenario/Composition/Object/Instantiate")]
    public class Composition_Instantiate : CompositionObject
    {
        // 연출
        [Header("Variables")]
        [SerializeField]
        GameObject targetPrefab;
        [SerializeField]
        float delayExcute = 1f;

        public override void Execute(GameObject obj)
        {
            StoryManager.Instance.CorExcutor(InstantiateCor(obj, delayExcute));
        }

        // 반복기
        IEnumerator InstantiateCor(GameObject obj, float delayExcute)
        {
            yield return new WaitForSeconds(delayExcute);
            Instantiate(targetPrefab, obj.transform.position, obj.transform.rotation);
        }
    }
}