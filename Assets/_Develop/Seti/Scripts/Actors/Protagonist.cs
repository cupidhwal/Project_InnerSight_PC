using JungBin;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    public class Protagonist : Storyteller
    {
        /*[Header("Dialogue List")]
        [SerializeField]
        private List<Dialogue> dialogues = new();*/

        public override int DialogueNumber()
        {
            return -1;
        }

        public override void StoryEnter()
        {
            

            OnStoryEnter?.Invoke();
        }
    }
}