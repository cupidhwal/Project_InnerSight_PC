using Noah;
using UnityEngine;

namespace Noah
{
    public class PlayerInfoManager : Singleton<PlayerInfoManager>
    {
        private int gold;

        public int startGold;

        public int Gold => gold;

        private void Start()
        {
            gold = startGold;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                GetGold(100000);
            }
        }

        public void GetGold(int _gold)
        {
            gold += _gold;
            UIManager.Instance.UpdateGoldUI();
        }

        public void SetGold(int _gold)
        {
            gold = _gold;
            UIManager.Instance.UpdateGoldUI();
        }

        public void UseGold(int _gold)
        {
            if(gold > 0)
            {
                gold -= _gold;
                UIManager.Instance.UpdateGoldUI();
            }
        }

        

    }
}