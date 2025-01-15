using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 아이템 데이터를 취합한 컨테이너
    /// </summary>
    [CreateAssetMenu(fileName = "New ItemDatabase", menuName = "Inventory System/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        // 필드
        #region Variables
        public ItemObject[] itemObjects;
        #endregion

        // 이벤트 메서드
        #region Event Methods
        // 인스펙터 창에서 값을 조정할 때마다 호출되는 함수
        // itemObjects의 item의 id값을 설정
        private void OnValidate()
        {
            for (int i = 0; i < itemObjects.Length; i++)
            {
                if (itemObjects[i] == null)
                    continue;

                itemObjects[i].data.id = i;
            }
        }
        #endregion
    }
}