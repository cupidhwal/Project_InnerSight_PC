using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 캐릭터 속성
    /// </summary>
    public enum CharAttribute
    {
        Agility,
        Intellect,
        Stamina,
        Strength,
        Health,
        Mana
    }

    /// <summary>
    /// 아이템 종류
    /// </summary>
    public enum ItemType : int
    {
        Helmet = 0,
        Chest = 1,
        Pants = 2,
        Boots = 3,
        Pauldrons = 4,
        Gloves = 5,
        LeftWeapon = 6,
        RightWeapon = 7,
        Food,
        Default,
    }
}