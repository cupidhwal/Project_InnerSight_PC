using UnityEngine;
using Unity.Cinemachine;

namespace Seti
{
    public class CameraFollow : MonoBehaviour
    {
        // 필드
        #region Variables
        private CinemachineCamera cinemachineCamera;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            cinemachineCamera = GetComponent<CinemachineCamera>();
            cinemachineCamera.Lens.NearClipPlane = -10;
            cinemachineCamera.Follow = FindFirstObjectByType<Player>().transform;
        }
        #endregion
    }
}