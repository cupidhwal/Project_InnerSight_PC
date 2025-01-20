using System;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 캐릭터 속성 타입, 값
    /// </summary>
    [Serializable]
    public class Attribute
    {
        public Attribute_Character type;
        public ModifiableInt value;
    }
}