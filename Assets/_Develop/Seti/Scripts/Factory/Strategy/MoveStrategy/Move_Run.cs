namespace Seti
{
    public class Move_Run : Move_Base
    {
        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);

            if (actor is Enemy enemy)
                speed = enemy.Magnification_WalkToRun * actor.Rate_Movement;
        }
    }
}