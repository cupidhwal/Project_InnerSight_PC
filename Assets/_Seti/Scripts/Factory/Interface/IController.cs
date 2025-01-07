using System;

namespace Seti
{
    public interface IController
    {
        Type GetControlType();
        void Initialize();
        void SetActorBehaviours(Actor actor);
    }
}