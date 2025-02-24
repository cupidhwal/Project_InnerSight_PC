using UnityEngine;

namespace Noah
{
    public class SkillReinforce : MonoBehaviour
    {
        private void Start()
        {
            //ObjectFadeSystem.Instance.ObjectFadeIn_Paritcle(transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
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
        }
    }

}
