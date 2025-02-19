using JungBin;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    [Serializable]
    public struct Dialogue
    {
        public string xmlPath;
        public int dialogueIndex;
        public bool isRead;
    }

    public class Protagonist : Storyteller
    {
        [Header("Dialogue List")]
        [SerializeField]
        private List<Dialogue> dialogues = new();

        public override void StoryEnter()
        {
            

            OnStoryEnter?.Invoke();
        }
    }
}