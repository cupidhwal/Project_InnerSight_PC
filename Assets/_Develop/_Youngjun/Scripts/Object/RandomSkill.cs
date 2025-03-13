using UnityEngine;

namespace Noah
{
    public class RandomSkill : MonoBehaviour
    {
        //ParticleSystem[] particleSystems;

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

                    GetRandomSkill();
                }
            }
        }

        void ActiveCollider()
        {
            transform.GetComponent<Collider>().enabled = true;
        }

        void GetRandomSkill()
        {
            transform.GetComponent<Collider>().enabled = false;

            UIManager.Instance.skillSelectUI.SetActive(true);
            InGameUI_Skill.instance.GetRandomSkill();
            Time.timeScale = 0f;

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isContact = true;

                actionUI_Text = "마기흡수";

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