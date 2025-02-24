using System.Collections;
using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New SetActive Action", menuName = "Scenario/Composition/Object/SetActive")]
    public class Composition_SetActive : CompositionObject
    {
        private enum ActiveFlag
        {
            True,
            False
        }

        // 연출
        [Header("Variables")]
        [SerializeField]
        ActiveFlag activeFlag;
        [SerializeField]
        float delayExcute = 1f;

        public bool Flag
        {
            get
            {
                bool flag = false;
                switch (activeFlag)
                {
                    case ActiveFlag.True:
                        flag = true;
                        break;

                    case ActiveFlag.False:
                        flag = false;
                        break;
                }
                return flag;
            }
        }

        public override void Execute(GameObject obj) => StoryManager.Instance.CorExcutor(Delay(obj));

        IEnumerator Delay(GameObject obj)
        {
            yield return new WaitForSeconds(delayExcute);
            obj.SetActive(Flag);
        }
    }
}