using System.Collections.Generic;
using UnityEngine;
using Noah;

namespace Seti
{
    /// <summary>
    /// 엔딩 트리거
    /// </summary>
    public class Trigger_End : Trigger_Stage
    {
        public override void OpenDialogue()
        {
            if (!SaveLoadManager.Instance.scenarioSaveData.flynneEvent[5])
                triggers.Remove(triggers[0]);
            else triggers.Remove(triggers[1]);

            base.OpenDialogue();
        }
    }
}