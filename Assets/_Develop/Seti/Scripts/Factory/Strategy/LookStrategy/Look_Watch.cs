using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Enemy - Player 주시 기능
    /// </summary>
    public class Look_Watch : Look_Base
    {
        // 오버라이드
        #region Override
        public override void Look(Vector2 _)
        {
            if (actor is Enemy enemy)
                enemy.transform.LookAt(enemy.Player.transform.position);
        }
        #endregion
    }
}