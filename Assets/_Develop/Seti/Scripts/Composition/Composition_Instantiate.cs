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
        Vector3 targetPosition;
        [SerializeField]
        float delayExcute = 1f;

        public override void Execute(GameObject _)
        {
            StoryManager.Instance.CorExcutor(InstantiateCor(delayExcute));
        }

        // 반복기
        IEnumerator InstantiateCor(float delayExcute)
        {
            yield return new WaitForSeconds(delayExcute);
            Instantiate(targetPrefab, targetPosition, Quaternion.Euler(new(0f, 90f, 0f)));
        }
    }
}