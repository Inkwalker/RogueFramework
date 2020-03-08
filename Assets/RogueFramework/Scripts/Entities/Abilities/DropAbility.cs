using UnityEngine;

namespace RogueFramework
{
    /// <summary>
    /// Ability to drop items.
    /// </summary>
    public class DropAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.ItemDrop;

        public override bool CanPerform(Actor user, Entity target)
        {
            var inv = user?.Entity.GetEntityComponent<Inventory>();
            var item = target?.GetEntityComponent<Item>();

            return base.CanPerform(user, target) && inv != null && item != null && inv.Contains(item);
        }

        public override bool CanPerform(Actor user, Vector2Int tile)
        {
            return false;
        }

        protected override ActorActionResult OnPerform(Actor user, Entity targetEntity, Vector2Int targetTile)
        {
            var inventory = user.Entity.GetEntityComponent<Inventory>();
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
