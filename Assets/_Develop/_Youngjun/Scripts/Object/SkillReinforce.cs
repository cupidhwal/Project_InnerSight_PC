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
        }

        private void Update()
        {
            if (isContact)
            {
                actionUI_Text = "마기강화";

                ActionUIManager.Instance.EnableActionUI(actionUI_Text);

                if (Input.GetKeyDown(KeyCode.G))
                {
                    GetSkillReinforce();
                }
            }
            else
            {
                ActionUIManager.Instance.DisableActionUI();
            }
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
