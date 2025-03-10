using System.Collections;
using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Destroy Action", menuName = "Scenario/Composition/Object/Destroy")]
    public class Composition_Destroy : CompositionObject
    {
        // 연출
        [Header("Variables")]
        [SerializeField]
        float delayExcute = 1f;

        public override void Execute(GameObject obj)
        {
            StoryManager.Instance.CorExcutor(DeathComposition(obj, delayExcute));
        }

        IEnumerator DeathComposition(GameObject obj, float delay)
        {
            DamageControl damage = obj.GetComponent<DamageControl>();
            Renderer renderer = damage.BodyRenderer;
            Material dissolve = damage.Dissolve;

            float dissolveDegree = 0.6f;
            renderer.material = new(dissolve);
            renderer.material.SetFloat("_Degree", dissolveDegree);

            yield return new WaitForSeconds(delay - 1);
            while (dissolveDegree >= -0.4f)
            {
                dissolveDegree -= Time.deltaTime;
                renderer.material.SetFloat("_Degree", dissolveDegree);
                yield return null;
            }
            Destroy(obj);
        }
    }
}