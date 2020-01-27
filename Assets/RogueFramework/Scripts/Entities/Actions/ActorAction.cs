using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public abstract class ActorAction
    {
        public Actor Actor { get; }

        public abstract int Cost { get; }

        public ActorAction(Actor actor)
        {
            Actor = actor;
        }

        public abstract ActorActionResult Perform();
    }
}
