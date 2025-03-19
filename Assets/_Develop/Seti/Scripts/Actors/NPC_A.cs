using Noah;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 주민 A 전용
    /// </summary>
    public class NPC_A : Storyteller_NPC
    {
        public override void StoryEnter()
        {
            if (SaveLoadManager.Instance.scenarioSaveData.flynneEvent[0]) return;

            base.StoryEnter();
        }
    }
}