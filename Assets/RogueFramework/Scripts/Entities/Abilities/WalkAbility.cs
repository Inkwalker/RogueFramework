using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class WalkAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.Move;

        public override bool CanPerform(Entity target)
        {
            return 
                base.CanPerform(target) &&
                target.Level != null &&
                target.Level == Owner.Level && 
                target.BlocksMovement == false;
        }

        public override bool CanPerform(Vector2Int tile)
        {
            return base.CanPerform(tile) && MapUtils.IsNeighborCells(Owner.Cell, tile) && Actor.CanOccupy(tile);
        }

        protected override ActorActionResult OnPerform(Entity targetEntity, Vector2Int targetTile)
        {
            Vector2Int delta = MapUtils.To4Dir(targetTile - Owner.Cell);
            Vector2Int cell = Owner.Cell + delta;

            if (Actor.CanOccupy(cell))
                Actor.Entity.Position += delta;

            return null;
        }
    }
}
