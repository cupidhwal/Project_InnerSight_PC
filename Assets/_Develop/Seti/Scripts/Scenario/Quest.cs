using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 퀘스트 데이터 리스트 클래스
    /// </summary>
    [Serializable]
    public class Quests
    {
        public List<Quest> quests { get; set; }
    }

    public enum QuestType
    {
        None,
        Kill,
        Collect
    }

    /// <summary>
    /// 퀘스트 데이터 클래스
    /// </summary>
    [Serializable]
    public class Quest
    {
        public int number { get; set; }             // 퀘스트 인덱스
        public int npcNumber { get; set; }          // 퀘스트를 가지고 있는 NPC
        public string name { get; set; }            // 퀘스트 이름
        public string description { get; set; }     // 퀘스트 내용
        public int dialogIndex { get; set; }        // 퀘스트 대화 내용 - 의뢰, 진행 중, 완료
        public int level { get; set; }              // 퀘스트 레벨 제한
        public QuestType questType { get; set; }    // 퀘스트 타입
        public int goalIndex { get; set; }          // 퀘스트 목표 아이템 타입, 에너미 타입
        public int goalAmount { get; set; }         // 퀘스트 목표 수량
        public int rewardGold { get; set; }         // 보상 골드
        public int rewardExp { get; set; }          // 보상 경험치
        public int rewardItem { get; set; }         // 보상 아이템
    }
}