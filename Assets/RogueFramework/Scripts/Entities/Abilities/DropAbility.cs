using UnityEngine;

namespace RogueFramework
{
    /// <summary>
    /// Ability to drop items.
    /// </summary>
    public class DropAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.ItemDrop;

        public override bool CanPerform(Entity target)
        {
            var inv = Owner?.GetEntityComponent<Inventory>();
            var item = target?.GetEntityComponent<Item>();

            return base.CanPerform(target) && inv != null && item != null && inv.Contains(item);
        }

        public override bool CanPerform(Vector2Int tile)
        {
            return false;
        }

        protected override ActorActionResult OnPerform(Entity targetEntity, Vector2Int targetTile)
        {
            var inventory = Owner.GetEntityComponent<Inventory>();
            var item = targetEntity.GetEntityComponent<Item>();

            if (inventory != null && item != null)
            {
                Debug.Log($"Dropping item {targetEntity.name}");
                inventory.Drop(item);
            }

            return null;
        }
    }
}
