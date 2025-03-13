using UnityEngine;

namespace Noah
{
    public class SkillReinforce : MonoBehaviour
    {
        private string actionUI_Text = "";
        private bool isContact = false;

        private void Start()
        {
            ObjectFadeSystem.Instance.ObjectFadeIn_Particle(transform);
            Invoke("ActiveCollider", 2f);
        }

        private void Update()
        {
            if (isContact)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    ActionUIManager.Instance.DisableActionUI();

                    GetSkillReinforce();
                }
            }
        }

        void ActiveCollider()
        {
            transform.GetComponent<Collider>().enabled = true;
        }

        void GetSkillReinforce()
        {
            transform.GetComponent<Collider>().enabled = false;

            UIManager.Instance.skillReinforce.SetActive(true);

            UIManager.Instance.skillReinforce.GetComponent<InGameUI_SkillReinforce>().SetSkill();

            Time.timeScale = 0f;

            if (HiddenStageManager.Instance != null)
            {
                HiddenStageManager.Instance.SelectReinforce();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isContact = true;

                actionUI_Text = "마기강화";

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
