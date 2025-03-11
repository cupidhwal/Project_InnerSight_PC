using JungBin;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 스토리에 필요한 NPC
    /// </summary>
    public abstract class Storyteller : MonoBehaviour
    {
        // 추상화
        public abstract void StoryEnter();
    }
}