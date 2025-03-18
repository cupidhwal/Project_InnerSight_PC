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
        Vector3 lastPos;

        private void Start()
        {
            ObjectFadeSystem.Instance.ObjectFadeIn_Particle(transform);

            if (!isHidden)
            {
                Invoke("ActiveCollider", 2f);
            }
            
        }

        private void Update()
        {
            if (isContact)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    ActionUIManager.Instance.DisableActionUI();

                    ChangeStage();

                    isContact = false;
                }
            }
        }

        void ActiveCollider()
        {
            transform.GetComponent<Collider>().enabled = true;
        }

        void ChangeStage()
        {
            AudioManager.Instance.Play("Telleport");

            transform.GetComponent<Collider>().enabled = false;

            GameManager.Instance.AnyChangeStage();

            if (isHidden)
            {
                StageManager.Instance.IsHidden = true;

                StageManager.Instance.playerPos = lastPos;

                Destroy(gameObject);
            }
            else if (escapeHidden)
            {
                HiddenStageManager.Instance.wasHidden = true;

                StageManager.Instance.ReturnCurrentStage();

                StageManager.Instance.reinforceData.Clear();

                return;
            }
            else
            {
                StageManager.Instance.IsHidden = false;
                GameManager.Instance.ChangeStage();
                StageManager.Instance.reinforceData.Clear();
            }

            StageManager.Instance.NextStage();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isContact = true;

                if (isHidden)
                {
                    lastPos = other.transform.position;
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
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isContact = false;

                ActionUIManager.Instance.DisableActionUI();
            }
        }
    }
    
}
