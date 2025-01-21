using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Seti
{
    /// <summary>
    /// 데이터: 이펙트 클립 리스트, 이펙트 데이터 파일 이름, 경로
    /// 기능 : 데이터 파일(xml) 읽기(Load), 쓰기(Save), 데이터 추가,복사,제거
    /// </summary>
    public class EffectData : BaseData
    {
        #region Variables
        public Effect effect;       //이펙트 클립 리스트

        private string xmlFilePath = string.Empty;
        private string xmlFileName = "effectData.xml";
        private string dataPath = "Data/effectData";
        #endregion

        //생성자
        public EffectData() { }

        //데이터 파일(xml) 읽기(Load)
        public void LoadData()
        {
            TextAsset asset = (TextAsset)ResourcesManager.Load(dataPath);
            if(asset == null || asset.text == null)
            {
                //새로운 데이터 추가
                AddData("NewEffect");
                return;
            }

            using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
            {
                var xs = new XmlSerializer(typeof(Effect));
                effect = (Effect)xs.Deserialize(reader);

                //데이터 셋팅
                int length = effect.effectClips.Count;
                this.names = new List<string>();
                for (int i = 0; i < length; i++)
                {
                    this.names.Add(effect.effectClips[i].name);
                }
            }

        }

        //데이터 파일(xml) 쓰기(Save)
        public void SaveData()
        {
            //데이터 저장 경로
            xmlFilePath = Application.dataPath + DataDirectory;
            Debug.Log($"xmlFilePath : {xmlFilePath}");

            using (XmlTextWriter xml = new XmlTextWriter(xmlFilePath + xmlFileName, System.Text.Encoding.Unicode))
            {
                var xs = new XmlSerializer (typeof(Effect));
                //저장할 내용 셋팅
                int length = effect.effectClips.Count;
                for (int i = 0;i < length; i++)
                {
                    effect.effectClips[i].id = i;
                    effect.effectClips[i].name = this.names[i];
                }

                xs.Serialize(xml, effect);
            }
        }

        //데이터 추가, 데이터 목록 갯수 반환
        public override int AddData(string newName)
        {
            //데이터 파일이 존재하지 않을 경우
            if(this.names == null)
            {
                this.names = new List<string>() { newName };

                effect = new Effect();
                effect.effectClips = new List<EffectClip>() { new EffectClip() };
            }
            else
            {
                this.names.Add(newName);
                effect.effectClips.Add(new EffectClip());
            }

            return GetDataCount();
        }

        //데이터 삭제
        public override void RemoveData(int index)
        {
            this.names.Remove(this.names[index]);
            if (this.names.Count == 0)
                this.names = null;

            this.effect.effectClips.Remove(this.effect.effectClips[index]);
            if(this.effect.effectClips.Count == 0)
            {
                this.effect.effectClips = null;
                this.effect = null;
            }
        }

        //데이터 복사, 현재 지정한 인덱스의 클립을 복사해서 추가
        public override void Copy(int index)
        {
            this.names.Add(this.names[index]);
            this.effect.effectClips.Add(GetCopy(index));
        }

        //데이터 복사
        public EffectClip GetCopy(int index)
        {
            //인덱스 체크
            if(index < 0 || index >= this.effect.effectClips.Count)
            {
                return null;
            }

            EffectClip original = this.effect.effectClips[index];

            EffectClip clip = new EffectClip();
            clip.effectName = original.effectName;
            clip.effectType = original.effectType;
            clip.effectPath = original.effectPath;
            return clip;
        }

        //모든 데이터 해제
        public void ClearData()
        {
            foreach (var clip in effect.effectClips)
            {
                clip.ReleaseEffect();
            }
            this.effect.effectClips = null;
            this.effect = null;
            this.names = null;
        }

        //현재 선택한 이펙트 클립 가져오기
        public EffectClip GetClip(int index)
        {
            //인덱스 체크
            if (index < 0 || index >= this.effect.effectClips.Count)
            {
                return null;
            }

            //프리팹 로드
            this.effect.effectClips[index].PreLoad();

            return this.effect.effectClips[index];
        }
    }
}