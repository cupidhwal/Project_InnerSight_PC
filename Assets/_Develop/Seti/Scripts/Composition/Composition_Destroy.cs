using System.Collections;
using System.Collections.Generic;
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
            Material[] dissolve = damage.Dissolve;
            Material[] newMaterials = new Material[dissolve.Length];

            float dissolveDegree = 0.6f;
            for (int i = 0; i < dissolve.Length; i++)
            {
                newMaterials[i] = new(dissolve[i]);
                newMaterials[i].SetFloat("_Degree", dissolveDegree);
            }
            renderer.SetMaterials(new List<Material>(newMaterials));

            yield return new WaitForSeconds(delay - 1);
            while (dissolveDegree >= -0.4f)
            {
                dissolveDegree -= Time.deltaTime;
                for (int i = 0; i < dissolve.Length; i++)
                    renderer.materials[i].SetFloat("_Degree", dissolveDegree);
                yield return null;
            }
            Destroy(obj);
        }
    }
}