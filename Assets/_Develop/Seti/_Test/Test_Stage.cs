using UnityEngine;

namespace Seti
{
    public class Test_Stage : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("활성");
        }

        private void OnDisable()
        {
            Debug.Log("비활성");
        }
    }
}