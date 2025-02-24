using MySampleEx;
using Noah;
using System.Threading.Tasks;
using UnityEngine;

namespace Seti
{
    [RequireComponent(typeof(Condition_Enemy_Tutorial))]
    public class Enemy_Tutorial : Enemy
    {
        [SerializeField]
        private GameObject award;

        protected override void Start()
        {
            base.Start();

            award = StageManager.Instance.transform.GetChild(0).GetChild(0).gameObject;
        }

        private void OnDestroy()
        {
            ClearTutorial();
        }

        // 오버라이드
        #region Override
        protected override Condition_Actor CreateState() => gameObject.AddComponent<Condition_Enemy_Tutorial>();
        #endregion

        // Tutotial 끝 / 대화 시작
        private void ClearTutorial()
        {
            award.SetActive(true);
        }
    }
}