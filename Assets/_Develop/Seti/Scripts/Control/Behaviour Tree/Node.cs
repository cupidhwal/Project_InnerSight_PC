namespace Seti
{
    /// <summary>
    /// Behaviour Tree의 기본
    /// </summary>
    public abstract class Node
    {
        protected Actor actor;
        protected Actor target;

        public abstract bool Execute();
    }
}