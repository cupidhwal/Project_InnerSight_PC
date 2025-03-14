using UnityEngine;
using JungBin;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Character Action", menuName = "Scenario/Composition/Character/Boss Start")]
    public class Composition_Control_Boss : CompositionObject
    {
        public override void Execute(GameObject obj)
        {
            if (obj.transform.TryGetComponent<LastBossManager>(out var boss))
            {
                boss.GetComponent<Animator>().SetBool("Start", true);
            }
        }
    }
}