using UnityEngine;

namespace Seti
{
    /// <summary>
    /// Enemy - Player 주시 기능
    /// </summary>
    public class Look_Watch : Look_Base
    {
        public override void Look(Vector2 _)
        {
            //if (!actor.Condition.IsAttack) return;

            if (actor is Enemy enemy && enemy.Player)
            {
                Controller_FSM enemyController = enemy.Controller as Controller_FSM;
                Condition_Enemy enemyCondition = enemy.Condition as Condition_Enemy;
                
                if (!enemyCondition.IsDead)
                {
                    if (enemyController.CurrentState == Controller_FSM.EnemyState.Encounter ||
                    enemyController.CurrentState == Controller_FSM.EnemyState.Attack_Normal ||
                    enemyCondition.IsMagic)
                    {
                        enemy.transform.LookAt(enemy.Player.transform.position);
                    }
                }

                //if (!enemyCondition.IsMove &&
                //    !enemyCondition.IsChase &&
                //    !enemyCondition.IsAttack &&
                //    !enemyCondition.IsPositioning &&
                //    !enemyCondition.IsDead ||
                //    enemyCondition.IsMagic)
                //{
                //    enemy.transform.LookAt(enemy.Player.transform.position);
                //    Debug.Log($"Enemy Watch : To Player");
                //}
            }

            if (actor is Player player)
            {
                if (player.Condition.IsAttack || player.Condition.IsMagic || player.Condition.IsSlash_Double || player.Condition.IsSlash_Multiple)
                {
                    Vector3 temp = player.Condition.AttackPoint;
                    float tempDis = Vector3.Distance(player.transform.position, temp);
                    if (tempDis > 1.5f)
                    {
                        Vector3 atkPoint = new(temp.x, player.transform.position.y, temp.z);
                        player.transform.LookAt(atkPoint);
                    }
                }
            }
        }
    }
}