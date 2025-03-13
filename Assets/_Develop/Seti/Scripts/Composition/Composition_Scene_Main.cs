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
            SceneFade sceneFade = FindAnyObjectByType<SceneFade>();

            sceneFade.FadeIn(sceneName, delay);
        }
    }
}