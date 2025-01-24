using UnityEngine;
using Unity.Cinemachine;

namespace Seti
{
    public class CameraFollow : MonoBehaviour
    {
        // 필드
        #region Variables
        private CinemachineCamera cinemachineCamera;
        [SerializeField]
        private float nearClipPlane = 0.3f;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            cinemachineCamera = GetComponent<CinemachineCamera>();
            cinemachineCamera.Lens.NearClipPlane = nearClipPlane;
            cinemachineCamera.Follow = FindFirstObjectByType<Player>().transform;
        }
        #endregion
    }
}