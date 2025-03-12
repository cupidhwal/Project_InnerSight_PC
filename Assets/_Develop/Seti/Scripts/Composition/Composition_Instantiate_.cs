using System.Collections;
using UnityEngine;
using Noah;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Instantiate Action", menuName = "Scenario/Composition/Object/Instantiate_")]
    public class Composition_Instantiate_ : CompositionObject
    {
        // 연출
        [Header("Variables")]
        [SerializeField]
        GameObject targetPrefab;
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
            Transform targetTransform = FindAnyObjectByType<Player>().transform;

            Vector3 targetPos = targetTransform.position + new Vector3(1f, 0f, 2f);
            Instantiate(targetPrefab, targetPos, Quaternion.Euler(new(0f, 90f, 0f)), StageManager.Instance.CurrentStage.transform);
        }
    }
}