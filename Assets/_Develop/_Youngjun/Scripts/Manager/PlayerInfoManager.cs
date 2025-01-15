using Noah;
using UnityEngine;

namespace Noah
{
    public class PlayerInfoManager : Singleton<PlayerInfoManager>
    {
        private int gold = 0;

        public int Gold => gold;

        public void GetGold(int _gold)
        {
            gold += _gold;
            UIManager.Instance.UpdateGoldUI();
        }


    }
}