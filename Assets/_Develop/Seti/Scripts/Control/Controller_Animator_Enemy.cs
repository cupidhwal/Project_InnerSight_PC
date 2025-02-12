using UnityEngine;

namespace Seti
{
    public class Controller_Animator_Enemy : Controller_Animator
    {
        // 필드
        private Enemy enemy;

        // 라이프 사이클
        protected override void Start()
        {
            base.Start();
            enemy = Actor as Enemy;
        }

        // 메서드
        public void Attack_Magic() => enemy.MagicAttack();
    }
}