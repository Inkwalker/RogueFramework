using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class AiActor : Actor
    {
        private Actor target;

        public override void OnLevelChanged()
        {
            target = null;

            if (Entity.Level != null)
            {
                var actors = Entity.Level.Entities.Actors;

                foreach (var item in actors)
                {
                    if (item is HeroActor)
                    {
                        target = item;
                        break;
                    }
                }
            }
        }

        public override ActorActionResult TakeTurn()
        {
            var move = GetAbility(AbilitySignature.Move);

            if (target != null && move != null)
            {
                if (MapUtils.IsNeighborCells(target.Entity.Cell, Entity.Cell) == false)
                {
                    var dir = target.Entity.Cell - Entity.Cell;
                    var delta = MapUtils.To4Dir(dir);

                    return move.Perform(this, Entity.Cell + delta);
                }
            }

            return null;
        }
    }
}
