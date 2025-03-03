using System.Collections;
using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Blink Action", menuName = "Scenario/Composition/Object/Blink")]
    public class Composition_Blink : CompositionObject
    {
        [SerializeField]
        float delayExcute = 1f;

        public override void Execute(GameObject obj) => StoryManager.Instance.CorExcutor(Delay(obj));

        IEnumerator Delay(GameObject obj)
        {
            obj.SetActive(!obj.activeSelf);
            yield return new WaitForSeconds(delayExcute);
            obj.SetActive(!obj.activeSelf);
            yield break;
        }
    }
}