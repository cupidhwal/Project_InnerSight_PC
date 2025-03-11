using Noah;

namespace Seti
{
    /// <summary>
    /// Storyteller - Flynne
    /// </summary>
    public class Flynne : Storyteller_NPC
    {
        public override bool StoryEnter()
        {
            if (StageManager.Instance.Enemies.Count > 0) return false;

            base.StoryEnter();
            return true;
        }
    }
}