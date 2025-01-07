using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Behaviour »óÀÚ
    /// </summary>
    [CreateAssetMenu(fileName = "Box_Behaviour", menuName = "Database/Box_Behaviour")]
    public class Box_Behaviour : ScriptableObject
    {
        [HideInInspector]
        [SerializeReference]
        public List<IBehaviour> behaviours;
    }
}