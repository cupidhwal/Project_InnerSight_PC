using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Seti
{
    /// <summary>
    /// Dialog 데이터 리스트
    /// </summary>
    [Serializable]
    [XmlRoot("Dialogues")]
    public class Dialogues
    {
        [XmlElement("Dialogue")]
        public List<Dialogue> dialogues;
    }

    /// <summary>
    /// Dialog 데이터 모델 클래스
    /// </summary>
    [Serializable]
    public class Dialogue
    {
        [XmlElement("number")]
        public int number;

        [XmlElement("order")]
        public int order;

        [XmlElement("character")]
        public int character;

        [XmlElement("name")]
        public string name;

        [XmlElement("sentence")]
        public string sentence;

        [XmlElement("nextType")]
        public NextType nextType;
    }

    public enum NextType
    {
        None = -1,
        Quest,
        Composition,
        Shop,
    }
}
