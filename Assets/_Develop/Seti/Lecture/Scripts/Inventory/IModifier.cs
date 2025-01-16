using UnityEngine;

namespace Seti
{
    public interface IModifier
    {
        // 매개변수로 입력받은 변수에 누적
        void AddValue(ref int baseValue);
    }
}