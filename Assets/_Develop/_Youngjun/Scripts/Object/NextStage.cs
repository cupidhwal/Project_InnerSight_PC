using Noah;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    public bool isHidden = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.GetComponent<Collider>().enabled = false;

            if (isHidden)
            {
                StageManager.Instance.IsHidden = true;
            }
            else
            {
                StageManager.Instance.IsHidden = false;
            }

            StageManager.Instance.NextStage();
        }
    }
}
