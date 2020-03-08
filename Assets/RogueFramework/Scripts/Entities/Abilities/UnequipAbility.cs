using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class UnequipAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.Undefined;

        public override bool CanPerform(Actor user, Entity target)
        {
            var item = target?.GetEntityComponent<Item>();
            var equipment = user?.Entity.GetEntityComponent<Equipment>();

            return
                base.CanPerform(user, target) &&
                item != null &&
                equipment != null &&
                equipment.Contains(item);
        }

        public override bool CanPerform(Actor user, Vector2Int tile)
        {
            return false;
        }

        protected override ActorActionResult OnPerform(Actor user, Entity targetEntity, Vector2Int targetTile)
        {
            Item targetItem = targetEntity.GetEntityComponent<Item>(); 

            if (targetItem == null)
            {
                Debug.Log($"Can't equip item at {targetTile}. Item have to be equipped.");
                return null;
            }

            var equipment = user.Entity.GetEntityComponent<Equipment>();

            if (equipment != null)
            {
                Debug.Log($"Unequipping item {targetEntity.name}");
                equipment.Remove(targetItem);

                var inv = user.Entity.GetEntityComponent<Inventory>();

                if (inv && inv.Add(targetItem))
                {
                }
                else
                {
                    Debug.Log($"Item {targetEntity.name} dropped.");
                    user.Entity.Level.Entities.Add(targetEntity);
                    targetEntity.Cell = user.Entity.Cell;
                }
            }
            else
            {
                Debug.Log($"Can't unequip item {targetEntity.name}. Actor doesn't have an equipment component");
            }
            

            return null;
        }
    }
}
