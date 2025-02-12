using Noah;
using System;
using UnityEngine;

[Serializable]
public class PlayerItem
{
    public int gold;


    public void ResetData()
    {
        if (!SaveLoadManager.Instance.isLoadData)
        {
            gold = PlayerInfoManager.Instance.startGold;
        }
        else
        {
            // 저장된 데이터가 있으면 적용
            PlayerItem loadedGold = SaveLoadManager.Instance.playerItem;

            gold = loadedGold.gold;
        }
    }
}
