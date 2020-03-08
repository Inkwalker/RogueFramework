using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class WalkAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.Move;

        public override bool CanPerform(Actor user, Entity target)
        {
            return 
                base.CanPerform(user, target) &&
                target.Level != null &&
                target.Level == Owner.Level && 
                target.BlocksMovement == false;
        }

        public override bool CanPerform(Actor user, Vector2Int tile)
        {
            return base.CanPerform(user, tile) && MapUtils.IsNeighborCells(user.Entity.Cell, tile) && user.CanOccupy(tile);
        }

        protected override ActorActionResult OnPerform(Actor user, Entity targetEntity, Vector2Int targetTile)
        {
            Vector2Int delta = MapUtils.To4Dir(targetTile - user.Entity.Cell);
            Vector2Int cell = user.Entity.Cell + delta;

            if (user.CanOccupy(cell))
                user.Entity.Position += delta;

            return null;
        }
    }
}
