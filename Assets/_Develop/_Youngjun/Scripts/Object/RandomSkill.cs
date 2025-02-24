using UnityEngine;

namespace Noah
{
    public class RandomSkill : MonoBehaviour
    {
        //ParticleSystem[] particleSystems;

        private void Start()
        {
            ObjectFadeSystem.Instance.ObjectFadeIn_Paritcle(transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                transform.GetComponent<Collider>().enabled = false;

                UIManager.Instance.skillSelectUI.SetActive(true);
                InGameUI_Skill.instance.GetRandomSkill();
                Time.timeScale = 0f;

                Destroy(gameObject);
            }
        }

    }
}