using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Actor 계층(Player, Enemy, etc...)을 구분하기 위한 기준
    /// </summary>
    public interface IActor
    {
        bool IsRelevant(Actor actor);
    }
}