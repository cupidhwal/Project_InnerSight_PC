using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Seti
{
    public class Effect
    {
        //public int length {  get; set; }
        public List<EffectClip> effectClips {  get; set; }
    }

    /// <summary>
    /// 이펙트 속성 데이터 : 이펙트 프리팹, 경로, 타입 등...
    /// 기능 : 프리팹 사전 로딩, 이펙트 인스턴스
    /// </summary>
    public class EffectClip
    {
        #region Variables
        public int id {  get; set; }                //이펙트 목록 인덱스
        public string name { get; set; }            //이펙트 이름
        public EffectType effectType { get; set; }  //이펙트 타입
        public string effectPath { get; set; }      //이펙트 프리팹 경로
        public string effectName { get; set; }      //이펙트 프리팹 이름

        private GameObject effectPrefab = null;
        #endregion

        //생성자
        public EffectClip() { }

        //프리팹 사전 로딩
        public void PreLoad()
        {
            if (effectPath == null || effectName == null)
                return;

            var effectFullPath = effectPath + effectName;
            if(effectFullPath != string.Empty && effectPrefab == null)
            {
                effectPrefab = ResourcesManager.Load(effectFullPath) as GameObject;
            }
        }

        //프리팹 해제
        public void ReleaseEffect()
        {
            if (effectPrefab != null)
            {
                effectPrefab = null;
            }
        }

        //이펙트 인스턴스
        public GameObject Instantiate(Vector3 position)
        {
            if (effectPrefab == null)
            {
                PreLoad();
            }
            if(this.effectPrefab != null)
            {
                GameObject effectGo = GameObject.Instantiate(effectPrefab, position, Quaternion.identity);
                return effectGo;
            }

            return null;
        }

    }
}
