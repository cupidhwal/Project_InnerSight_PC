using System;
using UnityEngine;

namespace Seti
{
    public enum NPCType
    {
        None = -1,
        Merchant,
        BlackSmith,
        SkillMaster,
        QuestGiver,
    }

    [Serializable]
    public class NPC
    {
        public NPCType npcType;     // NPC 타입
        public int number;          // NPC 고유번호
        public string name;         // NPC 이름
    }
}