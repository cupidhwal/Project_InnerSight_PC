using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour 상자
    /// </summary>
    [CreateAssetMenu(fileName = "Box_Behaviour", menuName = "Database/Box_Behaviour")]
    public class Box_Behaviour : ScriptableObject
    {
        [HideInInspector]
        [SerializeReference]
        public List<IBehaviour> behaviours;

        private void OnValidate()
        {
            // 삭제된 행동 클래스를 자동으로 정리
            int removedCount = behaviours.RemoveAll(b => b == null);

            // 삭제된 항목이 하나라도 있으면 강제로 재저장
            if (removedCount > 0)
            {
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets(); // 강제 저장
            }
        }
    }
}