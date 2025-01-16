namespace Seti
{
    public class Weapon_Tackle : Weapon
    {
        public override void AttackEnter() => BeginAttack(true);

        public override void AttackExit() => EndAttack();
    }
}