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
            if (actor is Enemy enemy && enemy.Player)
                enemy.transform.LookAt(enemy.Player.transform.position);

            if (actor is Player player)
            {
                Vector3 temp = player.Condition.AttactPoint;
                Vector3 atkPoint = new(temp.x, 0, temp.z);
                player.transform.LookAt(atkPoint);
            }
        }
        #endregion
    }
}