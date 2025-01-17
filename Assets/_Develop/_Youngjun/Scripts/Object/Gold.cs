using System;

namespace Noah
{
    [Serializable]
    public class Gold
    {
        public int hp_UpgradeGold;
        public int atk_UpgradeGold;
        public int def_UpgradeGold;
        public int moveSpeed_UpgradeGold;
        public int atkSpeed_UpgradeGold;

        public void LoadData(bool isLoad)
        {
            if (!isLoad)
            {
                hp_UpgradeGold = PlayerStateManager.Instance.upgradeGold.hp_UpgradeGold;
                atk_UpgradeGold = PlayerStateManager.Instance.upgradeGold.atk_UpgradeGold;
                def_UpgradeGold = PlayerStateManager.Instance.upgradeGold.def_UpgradeGold;
                moveSpeed_UpgradeGold = PlayerStateManager.Instance.upgradeGold.moveSpeed_UpgradeGold;
                atkSpeed_UpgradeGold = PlayerStateManager.Instance.upgradeGold.atkSpeed_UpgradeGold;
            }

        }
    }

}
