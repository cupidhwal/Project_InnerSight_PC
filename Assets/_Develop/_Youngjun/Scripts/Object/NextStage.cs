using InnerSight_Kys;
using JungBin;
using UnityEngine;

namespace Noah
{ 
    public class NextStage : MonoBehaviour
    {
        public bool escapeHidden = false;
        public bool isHidden = false;

        private string actionUI_Text = "";
        private bool isContact = false;
        Transform lastPos;

        private void Start()
        {
            ObjectFadeSystem.Instance.ObjectFadeIn_Particle(transform);
        }

        private void Update()
        {
            if (isContact)
            {
                if (isHidden)
                {
                    actionUI_Text = "히든 스테이지";
                }
                else if (escapeHidden)
                {
                    actionUI_Text = "스테이지 복귀";
                }
                else
                {
                    actionUI_Text = "스테이지 이동";
                }

                ActionUIManager.Instance.EnableActionUI(actionUI_Text);

                if (Input.GetKeyDown(KeyCode.G))
                {
                    ChangeStage();
                }
            }
            else
            {
                ActionUIManager.Instance.DisableActionUI();
            }
        }

        void ChangeStage()
        {
            AudioManager.Instance.Play("Telleport");

            transform.GetComponent<Collider>().enabled = false;

            GameManager.Instance.ChangeStage();

            if (isHidden)
            {
                StageManager.Instance.IsHidden = true;

                StageManager.Instance.playerPos = lastPos.position;

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


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (isHidden)
                {
                    lastPos.position = other.transform.position;
                }

                Debug.Log("1");

                isContact = true;
                Debug.Log("2");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isContact = false;
            }
        }
    }
    
}
