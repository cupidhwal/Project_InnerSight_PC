using Seti;
using UnityEngine;

[CreateAssetMenu(fileName = "New Destroy Action", menuName = "Scenario/Composition/Object/Destroy")]
public class Composition_Destroy : CompositionObject
{
    // 연출
    [Header("Variables")]
    [SerializeField]
    float delayExcute = 1f;

    public override void Execute(GameObject obj)
    {
        Destroy(obj, delayExcute);
    }
}