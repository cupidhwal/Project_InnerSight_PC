using UnityEngine;

namespace Noah
{
    public class HiddenStageManager : Singleton<HiddenStageManager>
    {
        private GameObject skillReinObj;
        private GameObject statsReinObj;

        public bool wasHidden = false;

        public void SetObject()
        {
            Transform groupObj = transform.parent.GetChild(0);

            skillReinObj = groupObj.GetChild(1).gameObject;
            statsReinObj = groupObj.GetChild(2).gameObject;
        }

        public void SelectReinforce()
        {
            Destroy(skillReinObj);
            Destroy(statsReinObj);
        }



    }
}
