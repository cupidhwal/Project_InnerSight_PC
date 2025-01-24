using Noah;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.GetComponent<Collider>().enabled = false;
            StageManager.Instance.NextStage();
        }
    }
}
