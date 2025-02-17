using UnityEngine;

namespace Noah
{
    public class SkillReinforce : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                transform.GetComponent<Collider>().enabled = false;

                UIManager.Instance.skillReinforce.SetActive(true);

                UIManager.Instance.skillReinforce.GetComponent<InGameUI_SkillReinforce>().SetSkill();

                Time.timeScale = 0f;

                Destroy(gameObject);
            }
        }
    }

}
