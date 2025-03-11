using InnerSight_Kys;
using JungBin;
using UnityEngine;

namespace Noah
{ 
    public class NextStage : MonoBehaviour
    {
        public bool escapeHidden = false;
        public bool isHidden = false;

        private void Start()
        {
            ObjectFadeSystem.Instance.ObjectFadeIn_Particle(transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AudioManager.Instance.Play("Telleport");

                transform.GetComponent<Collider>().enabled = false;

                GameManager.Instance.ChangeStage();

                if (isHidden)
                {
                    StageManager.Instance.IsHidden = true;

                    StageManager.Instance.playerPos = other.transform.position;

                    Destroy(gameObject);
                }
                else if (escapeHidden)
                {
                    HiddenStageManager.Instance.wasHidden = true;

                    StageManager.Instance.ReturnCurrentStage();

                    return;
                }
                else
                {
                    StageManager.Instance.IsHidden = false;
                }

                StageManager.Instance.NextStage();
            }
        }
    }
    
}
