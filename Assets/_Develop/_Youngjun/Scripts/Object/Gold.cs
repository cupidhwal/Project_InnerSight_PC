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

        public void ResetData()
        {

            hp_UpgradeGold = PlayerStatsManager.Instance.upgradeGold.hp_UpgradeGold;
            atk_UpgradeGold = PlayerStatsManager.Instance.upgradeGold.atk_UpgradeGold;
            def_UpgradeGold = PlayerStatsManager.Instance.upgradeGold.def_UpgradeGold;
            moveSpeed_UpgradeGold = PlayerStatsManager.Instance.upgradeGold.moveSpeed_UpgradeGold;
            atkSpeed_UpgradeGold = PlayerStatsManager.Instance.upgradeGold.atkSpeed_UpgradeGold;
            

        }
    }

}
