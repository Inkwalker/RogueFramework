using UnityEngine;

namespace RogueFramework
{
    /// <summary>
    /// Ability to pick up items.
    /// </summary>
    public class TakeAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.ItemTake;

        public override bool CanPerform(Actor user, Entity target)
        {
            var item = target.GetEntityComponent<Item>();

            return
                base.CanPerform(user, target) &&
                item != null &&
                item.InInventory == false;
        }

        public override bool CanPerform(Actor user, Vector2Int tile)
        {
            return base.CanPerform(user, tile) && user.Entity.Level?.Entities.Get<Item>(tile) != null;
        }

        protected override ActorActionResult OnPerform(Actor user, Entity target, Vector2Int tile)
        {
            Item targetItem = null;

            if (target == null)
            {
                targetItem = user.Entity.Level.Entities.Get<Item>(tile);
            }
            else
            {
                targetItem = target.GetEntityComponent<Item>();
            }

            if (targetItem == null)
            {
                Debug.Log($"Can't take item at {tile}. No item found.");
                return null;
            }

            Vector2Int actorCell = user.Entity.Cell;
            Vector2Int itemCell  = targetItem.Entity.Cell;

            if (actorCell == itemCell || MapUtils.IsNeighborCells(actorCell, itemCell))
            {
                var inv = user.Entity.GetEntityComponent<Inventory>();

                if (inv != null)
                {
                    LogView.Log($"{Actor.name} grabs {target.name}");
                    inv.Add(targetItem);
                }
                else
                {
                    Debug.Log($"Can't grab item {target.name}. Actor doesn't have inventory");
                }
            }

            return null;
        }
    }
}
