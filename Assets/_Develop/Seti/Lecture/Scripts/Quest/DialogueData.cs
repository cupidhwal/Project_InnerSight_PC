using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    public class DialogueData : ScriptableObject
    {
        // 필드
        #region Variables
        public Dialogues Dialogues;     // 대화 데이터베이스
        private string xmlFilePath = string.Empty;
        private string dataPath = "";
        #endregion

        // 생성자
        #region Constructor
        public DialogueData() { }
        #endregion

        // 메서드
        #region Methods
        //데이터 읽기
        public void LoadData()
        {
            dataPath = StoryManager.Instance.CurrentDialogue;
            TextAsset asset = (TextAsset)ResourcesManager.Load(dataPath);
            if (asset == null || asset.text == null)
            {
                return;
            }
            using XmlTextReader reader = new(new StringReader(asset.text));
            var xs = new XmlSerializer(typeof(Dialogues));
            Dialogues = (Dialogues)xs.Deserialize(reader);
        }
        #endregion
    }
}