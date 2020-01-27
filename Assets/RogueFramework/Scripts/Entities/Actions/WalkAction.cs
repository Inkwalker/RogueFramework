using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class WalkAction : ActorAction
    {
        private Vector2Int delta;

        public override int Cost => 4;

        public WalkAction(Actor actor, Vector2Int delta) : base(actor)
        {
            this.delta = delta;
        }

        public override ActorActionResult Perform()
        {
            var cell = Actor.Entity.Cell + delta;
            if (Actor.CanOccupy(cell))
                Actor.Entity.Position += delta;

            return null;
        }
    }
}
