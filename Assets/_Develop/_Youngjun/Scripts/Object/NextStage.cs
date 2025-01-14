using Noah;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.NextStage();
        }
    }
}
