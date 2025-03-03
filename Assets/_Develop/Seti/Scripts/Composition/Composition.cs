using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    [Serializable]
    public class Composition
    {
        // 필드
        [SerializeField]
        private string iD;
        public GameObject target;
        [SerializeField]
        private List<CompositionObject> actions;

        // 속성
        public string ID => iD;
        public GameObject Target
        {
            get
            {
                if (target == null)
                {
                    target = StoryManager.Instance.TempTarget;
                }
                return target;
            }
        }
        public List<CompositionObject> Actions => actions;
    }

    [Serializable]
    public class CompositionsPerScene
    {
        // 필드
        public int sceneIndex;
        public List<Composition> compositions;

        // 인스펙터에서 값이 변경될 때 자동 실행
        public void UpdateIndex(int index)
        {
            sceneIndex = index;
        }
    }
}