using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class GrabAction : ActorAction
    {
        private Item target;

        public GrabAction(Actor actor, Item item) : base(actor)
        {
            target = item;
        }

        public override int Cost => 1;

        protected override ActorActionResult OnPerform()
        {
            Vector2Int actorCell = Actor.Entity.Cell;
            Vector2Int itemCell = target.Entity.Cell;

            if (actorCell == itemCell || MapUtils.IsNeighborCells(actorCell, itemCell))
            {
                Debug.Log($"Grabbing item {target.name}");

                Object.Destroy(target.gameObject);
            }

            return null;
        }
    }
}
