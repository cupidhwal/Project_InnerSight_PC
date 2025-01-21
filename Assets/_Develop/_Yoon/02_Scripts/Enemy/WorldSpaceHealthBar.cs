using UnityEngine;
using UnityEngine.UI;

namespace Yoon
{

    public class WorldSpaceHealthBar : MonoBehaviour
    {
        #region Variables
        //public EnemyHealth health;

        public Image healthBarImage;

        public Transform healthBarPivot;

        //hp가 풀이면 healthbar를 숨긴다
        [SerializeField] private bool hideFullHealthBar = true;

        #endregion
        private void Update()
        {
            //healthBarImage.fillAmount = health.GetRatio();

            //UI가 플레이어를 바라보도록 한다.
            healthBarPivot.LookAt(Camera.main.transform.position);

            //hp가 풀이면 healthbar를 숨긴다
            if (hideFullHealthBar)
            {
            healthBarPivot.gameObject.SetActive(healthBarImage.fillAmount != 1);

            }

        }
    }

}