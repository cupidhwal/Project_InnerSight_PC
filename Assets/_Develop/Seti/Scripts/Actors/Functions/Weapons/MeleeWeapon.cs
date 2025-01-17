namespace Seti
{
    /// <summary>
    /// 
    /// </summary>
    public class MeleeWeapon : Weapon
    {
        // 오버라이드
        #region Override
        public override void AttackEnter() => BeginAttack(true);
        public override void AttackExit() => EndAttack();
        #endregion
    }
}