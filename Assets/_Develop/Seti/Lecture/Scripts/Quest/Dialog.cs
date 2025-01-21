using System;
using System.Collections.Generic;

namespace Seti
{
    /// <summary>
    /// Dialog 데이터 리스트
    /// </summary>
    [Serializable]
    public class Dialogs
    {
        public List<Dialog> dialogs;
    }

    /// <summary>
    /// Dialog 데이터 모델 클래스
    /// </summary>
    [Serializable]
    public class Dialog
    {
        public int number;
        public int character;
        public string name;
        public string sentence;
        public NextType nextType;
    }

    public enum NextType
    {
        None = -1,
        Quest,
        Shop,
    }
}
