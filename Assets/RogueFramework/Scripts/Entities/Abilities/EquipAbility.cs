using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class EquipAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.Undefined;

        public override bool CanPerform(Actor user, Entity target)
        {
            var item = target?.GetEntityComponent<Item>();
            var equipment = user?.Entity.GetEntityComponent<Equipment>();

            return
                base.CanPerform(user, target) &&
                item != null &&
                item.Type.Equippable &&
                equipment != null &&
                equipment.Contains(item) == false;
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
                Debug.Log($"Can't equip item at {targetTile}. No item found.");
                return null;
            }

            if (targetItem.Type.Equippable)
            {
                var equipment = user.Entity.GetEntityComponent<Equipment>();

                if (equipment != null)
                {
                    Debug.Log($"Equipping item {targetEntity.name}");
                    equipment.Add(targetItem);
                }
                else
                {
                    Debug.Log($"Can't equip item {targetEntity.name}. Actor doesn't have an equipment component");
                }
            }

            return null;
        }
    }
}
