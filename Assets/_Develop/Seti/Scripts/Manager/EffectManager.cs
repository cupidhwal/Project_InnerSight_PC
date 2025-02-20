using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 이펙트 데이터 기능 구현 
    /// </summary>
    public class EffectManager : Singleton<EffectManager>
    {
        private Transform effectRoot = null;

        private void Start()
        {
            if (effectRoot == null)
            {
                effectRoot = new GameObject("EffectRoot").transform;
                effectRoot.SetParent(this.transform);
            }
        }

        //이펙트 플레이
        public GameObject EffectOneShot(int index, Vector3 position)
        {
            EffectClip clip = DataManager.GetEffectData().GetClip(index);
            if(clip == null)
            {
                return null;
            }

            GameObject effectInstance = clip.Instantiate(position);
            effectInstance.SetActive(true);
            return effectInstance;
        }
    }
}
