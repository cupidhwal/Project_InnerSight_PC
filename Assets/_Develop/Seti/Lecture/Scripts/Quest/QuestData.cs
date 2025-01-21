using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Seti
{
    public class QuestData : ScriptableObject
    {
        // 필드
        #region Variables
        public Quests Quests;     // 대화 데이터베이스
        private string xmlFilePath = string.Empty;
        private string dataPath = "Data/DialogData";
        #endregion

        // 생성자
        #region Constructor
        public QuestData() { }
        #endregion

        // 메서드
        #region Methods
        //데이터 읽기
        public void LoadData()
        {
            TextAsset asset = (TextAsset)ResourcesManager.Load(dataPath);
            if (asset == null || asset.text == null)
            {
                return;
            }
            using XmlTextReader reader = new(new StringReader(asset.text));
            var xs = new XmlSerializer(typeof(Quests));
            Quests = (Quests)xs.Deserialize(reader);
        }
        #endregion
    }
}