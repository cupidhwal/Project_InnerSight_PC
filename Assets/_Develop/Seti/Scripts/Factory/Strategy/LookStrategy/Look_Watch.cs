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
            if (!actor.Condition.IsAttack) return;

            if (actor is Enemy enemy && enemy.Player)
                enemy.transform.LookAt(enemy.Player.transform.position);

            if (actor is Player player)
            {
                Vector3 temp = player.Condition.AttackPoint;
                float tempDis = Vector3.Distance(player.transform.position, temp);
                if (tempDis > 1.5f)
                {
                    Vector3 atkPoint = new(temp.x, temp.y, temp.z);
                    player.transform.LookAt(atkPoint);
                }
            }
        }
        #endregion
    }
}