using Noah;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    public class SetTarget : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StoryManager.Instance.SetTarget(StageManager.Instance.CurrentStage.transform.GetChild(0).GetChild(0).gameObject);
            }
        }
    }
}