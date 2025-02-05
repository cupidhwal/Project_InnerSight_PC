using UnityEngine;

namespace Seti
{
    public class MinimapControl : MonoBehaviour
    {
        // 필드
        #region Variables
        [SerializeField]
        private Player player;
        private Transform playerIcon;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            playerIcon = transform.GetChild(0);
        }

        private void LateUpdate()
        {
            ViewPort();
            SyncRotation_Icon();
        }
        #endregion

        // 메서드
        #region Methods
        private void ViewPort()
        {
            // 플레이어의 위치를 따라감, y 값은 고정
            Vector3 newPosition = player.transform.position /*+ new Vector3(0, 0, 1000)*/;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }

        private void SyncRotation_Icon()
        {
            //if (player.playerStates.isBoard == null)
                playerIcon.localRotation = Quaternion.Euler(0, 0, 45f - player.transform.rotation.eulerAngles.y);
            /*else playerIcon.localRotation = Quaternion.Euler(0,
                                                             0,
                                                             -player.ridingGear.transform.rotation.eulerAngles.y);*/
        }
        #endregion
    }
}