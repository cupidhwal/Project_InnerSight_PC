using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 현재 오브젝트를 특정 오브젝트의 위치에 부착
    /// </summary>
    [DefaultExecutionOrder(9999)]
    public class UpdateFollow_AddOn : MonoBehaviour
    {
        // 필드
        [SerializeField]
        private Transform toFollow;      //부착할 대상

        // 라이프 사이클
        private void Start()
        {
            //toFollow = GetComponentInParent<Actor>().GetComponentInChildren<Hand_Equip_Weapon>().transform;
        }

        private void Update()
        {
            transform.position = toFollow.position;
            transform.rotation = toFollow.rotation;
        }
    }
}