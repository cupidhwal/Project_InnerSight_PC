using System.Collections;
using UnityEngine;
using Noah;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Scene Change Action", menuName = "Scenario/Composition/Scene/Scene Change")]
    public class Composition_Scene_Main : CompositionObject
    {
        //필드
        [Header("Variables")]
        [SerializeField]
        private string sceneName = "MainMenu";
        [SerializeField]
        private float delay = 5f;

        public override void Execute(GameObject _)
        {
            
        }

        IEnumerator DelayExecute(float delay)
        {
            SceneFade sceneFade = FindAnyObjectByType<SceneFade>();

            sceneFade.FadeIn(sceneName, delay);

            yield return new WaitForSeconds(5);

            StoryManager.Instance.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 9;

            if (InitializeManager.Instance.Player.Controller.BehaviourMap.TryGetValue(typeof(Interact), out var behaviour))
                if (behaviour is Interact interact)
                    interact.OnInteraction();

            yield break;
        }
    }
}