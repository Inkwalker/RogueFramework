using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class EquipAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.Undefined;

        public override bool CanPerform(Entity target)
        {
            var item = target?.GetEntityComponent<Item>();

            return base.CanPerform(target) && item != null && item.Type.Equippable;
        }

        public override bool CanPerform(Vector2Int tile)
        {
            return false;
        }

        protected override ActorActionResult OnPerform(Entity targetEntity, Vector2Int targetTile)
        {
            Item targetItem = targetEntity.GetEntityComponent<Item>(); 

            if (targetItem == null)
            {
                Debug.Log($"Can't equip item at {targetTile}. No item found.");
                return null;
            }

            if (targetItem.Type.Equippable)
            {
                var equipment = Owner.GetEntityComponent<Equipment>();

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
