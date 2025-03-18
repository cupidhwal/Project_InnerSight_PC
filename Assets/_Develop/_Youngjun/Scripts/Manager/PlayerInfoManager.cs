using Noah;
using UnityEngine;

namespace Noah
{
    public class PlayerInfoManager : Singleton<PlayerInfoManager>
    {
        public int startGold;

        PlayerItem playerItem;

        private void Start()
        {
            playerItem = SaveLoadManager.Instance.playerItem;

            UIManager.Instance.Init();
        }

        public int GetGold()
        {
            return playerItem.gold;
        }

        public void AddGold(int _gold)
        {
            playerItem.gold += _gold;
            UIManager.Instance.UpdateGoldUI();
        }

        public void SetGold(int _gold)
        {
            playerItem.gold = _gold;
            UIManager.Instance.UpdateGoldUI();
        }

        public void UseGold(int _gold)
        {
            if(playerItem.gold > 0)
            {
                playerItem.gold -= _gold;
                UIManager.Instance.UpdateGoldUI();
            }
        }

        

    }
}