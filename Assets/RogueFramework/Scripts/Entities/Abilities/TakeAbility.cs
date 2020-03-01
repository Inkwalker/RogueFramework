using UnityEngine;

namespace RogueFramework
{
    /// <summary>
    /// Ability to pick up items.
    /// </summary>
    public class TakeAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.ItemTake;

        public override bool CanPerform(Entity target)
        {
            var item = target.GetEntityComponent<Item>();

            return
                base.CanPerform(target) &&
                item != null &&
                item.InInventory == false;
        }

        public override bool CanPerform(Vector2Int tile)
        {
            return base.CanPerform(tile) && Owner.Level?.Entities.Get<Item>(tile) != null;
        }

        protected override ActorActionResult OnPerform(Entity target, Vector2Int tile)
        {
            Item targetItem = null;

            if (target == null)
            {
                targetItem = Owner.Level.Entities.Get<Item>(tile);
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

            Vector2Int actorCell = Owner.Cell;
            Vector2Int itemCell  = targetItem.Entity.Cell;

            if (actorCell == itemCell || MapUtils.IsNeighborCells(actorCell, itemCell))
            {
                var inv = Owner.GetEntityComponent<Inventory>();

                if (inv != null)
                {
                    Debug.Log($"Grabbing item {target.name}");
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
