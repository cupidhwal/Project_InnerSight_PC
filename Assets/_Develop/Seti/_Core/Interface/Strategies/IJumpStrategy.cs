namespace Seti
{
    /// <summary>
    /// Jump Behaviour¿« Strategy Pattern
    /// </summary>
    public interface IJumpStrategy : IStrategy
    {
        void Initialize(Actor actor, float jumpForce);
        public void Jump();
    }
}