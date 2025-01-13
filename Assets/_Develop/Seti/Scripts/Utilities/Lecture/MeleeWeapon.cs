using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    public class MeleeWeapon : MonoBehaviour
    {
        /// <summary>
        /// 무기 공격 시 상대에게 입히는 피해의 구성
        /// </summary>
        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform attackRoot;

#if UNITY_EDITOR
            public List<Vector3> previousPositions = new();
#endif
        }

        // 필드
        #region Variables
        private int damage = 1;      // hit 시 데미지

        public AttackPoint[] attackPoints = new AttackPoint[0];
        public TimeEffect[] effects;
        #endregion

        // 속성
        #region Properties
        public int Damage => damage;
        #endregion

        // 메서드
        #region Methods
        #endregion

        // 유틸리티
        #region Utilities
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint attackPoint = attackPoints[i];

                if (attackPoint.attackRoot != null)
                {
                    Vector3 worldPos = attackPoint.attackRoot.TransformVector(attackPoint.offset);
                    Gizmos.color = new(1.0f, 1.0f, 1.0f, 0.4f);
                    Gizmos.DrawSphere(attackPoint.attackRoot.position + worldPos, attackPoint.radius);
                }

                if (attackPoint.previousPositions.Count > 0)
                {
                    UnityEditor.Handles.DrawAAPolyLine(10, attackPoint.previousPositions.ToArray());
                }
            }
        }
#endif
        #endregion
    }
}