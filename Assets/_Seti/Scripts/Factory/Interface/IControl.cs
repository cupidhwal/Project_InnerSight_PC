namespace Seti
{
    /// <summary>
    /// Control Interface
    /// </summary>
    public interface IControl
    {
        void OnEnter(Actor actor);  // 초기화
        void OnExit(Actor actor);   // 정리
    }
}